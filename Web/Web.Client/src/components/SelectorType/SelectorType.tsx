import 'reflect-metadata';

import { Box, Checkbox, FormControl, InputLabel, ListItemText, MenuItem, OutlinedInput, Select } from '@mui/material';
import { SelectChangeEvent } from '@mui/material/Select';
import { observer } from 'mobx-react';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { Type } from 'models';

interface Properties {
  label: string;
  items: Type[];
  selectedTypeIds: number[];
  minWidth?: number;
  onChange?: (types: number[] | string[]) => void;
}

const RESET_INDEX = -1;

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
  PaperProps: {
    style: {
      maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
      width: 250,
    },
  },
};

const SelectorType = observer(
  ({ label, items, selectedTypeIds, onChange, minWidth = 150 }: Properties): JSX.Element => {
    //const store = useInjection<ProductsStore>(IoCTypes.productsStore);
    const navigate = useNavigate();
    const { t } = useTranslation(['products']);

    const defaultValue = selectedTypeIds ? selectedTypeIds.map(String) : [];
    const [typesValue, setTypesValue] = useState<string[]>(defaultValue);

    const handleChange = (event: SelectChangeEvent<typeof typesValue>): void => {
      const {
        target: { value },
      } = event;

      const urlParameters = new URLSearchParams(window.location.search);

      const newValue = value.includes(RESET_INDEX.toString())
        ? []
        : typeof value === 'string'
        ? value.split(',')
        : value;

      setTypesValue(newValue);

      urlParameters.delete('page');

      if (newValue.length > 0) {
        urlParameters.set('types', newValue.toString());
      } else {
        urlParameters.delete('types');
      }

      if (onChange !== undefined) onChange(newValue);

      //store.changeTypeIds(newValue);

      navigate('?' + urlParameters.toString(), { replace: false, preventScrollReset: true });
    };

    return (
      <Box sx={{ minWidth: { minWidth } }}>
        <FormControl fullWidth>
          <InputLabel>{label}</InputLabel>
          <Select
            multiple
            id="type-selector"
            value={typesValue}
            label={label}
            onChange={handleChange}
            input={<OutlinedInput label={label} />}
            renderValue={(selected): string => {
              return t('selectors.selected_types', { count: selected.length });
            }}
            MenuProps={MenuProps}
          >
            <MenuItem key={RESET_INDEX.toString()} value={RESET_INDEX.toString()}>
              <em>
                <ListItemText primary={t('selectors.all')} />
              </em>
            </MenuItem>
            {items &&
              items?.map((item) => (
                <MenuItem key={item.id.toString()} value={item.id.toString()}>
                  <Checkbox checked={typesValue?.includes(item.id.toString())} />
                  <ListItemText primary={item.name} />
                </MenuItem>
              ))}
          </Select>
        </FormControl>
      </Box>
    );
  }
);

export default SelectorType;
