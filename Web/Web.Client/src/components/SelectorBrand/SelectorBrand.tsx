import 'reflect-metadata';

import {
  Box,
  Checkbox,
  FormControl,
  InputLabel,
  ListItemText,
  MenuItem,
  OutlinedInput,
  Select,
  SelectChangeEvent,
} from '@mui/material';
import { observer } from 'mobx-react';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { Brand } from 'models';

interface Properties {
  label: string;
  items: Brand[];
  selectedBrandIds: number[];
  minWidth?: number;
  onChange?: (brands: number[] | string[]) => void;
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

const SelectorBrand = observer(
  ({ label, items, selectedBrandIds, onChange, minWidth = 150 }: Properties): JSX.Element => {
    //const store = useInjection<ProductsStore>(IoCTypes.productsStore);
    const navigate = useNavigate();
    const { t } = useTranslation(['products']);

    const defaultValue = selectedBrandIds ? selectedBrandIds.map(String) : [];
    const [brandsValue, setBrandsValue] = useState<string[]>(defaultValue);

    const handleChange = (event: SelectChangeEvent<typeof brandsValue>): void => {
      const {
        target: { value },
      } = event;

      const urlParameters = new URLSearchParams(window.location.search);

      const newValue = value.includes(RESET_INDEX.toString())
        ? []
        : typeof value === 'string'
        ? value.split(',')
        : value;

      setBrandsValue(newValue);

      urlParameters.delete('page');

      if (newValue.length > 0) {
        urlParameters.set('brands', newValue.toString());
      } else {
        urlParameters.delete('brands');
      }

      if (onChange !== undefined) onChange(newValue);

      //store.changeBrandIds(newValue);

      navigate('?' + urlParameters.toString(), { replace: false, preventScrollReset: true });
    };

    return (
      <Box sx={{ minWidth: { minWidth } }}>
        <FormControl fullWidth>
          <InputLabel>{label}</InputLabel>
          <Select
            multiple
            id="brand-selector"
            value={brandsValue}
            label={label}
            onChange={handleChange}
            input={<OutlinedInput label={label} />}
            renderValue={(selected): string => {
              return t('selectors.selected_brands', { count: selected.length });
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
                  <Checkbox checked={brandsValue?.includes(item.id.toString())} />
                  <ListItemText primary={item.name} />
                </MenuItem>
              ))}
          </Select>
        </FormControl>
      </Box>
    );
  }
);

export default SelectorBrand;
