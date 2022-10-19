import 'reflect-metadata';

import { ButtonGroup, TextField } from '@mui/material';
import Button from '@mui/material/Button';
import { Box, Stack } from '@mui/system';
import { useInjection } from 'inversify-react';
import { observer } from 'mobx-react';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { IoCTypes } from 'ioc';
import { ProductsStore } from 'stores';

const LimitChangerButton = observer((): JSX.Element => {
  const store = useInjection<ProductsStore>(IoCTypes.productsStore);
  const navigate = useNavigate();
  const { t } = useTranslation(['products']);

  const defaultValue = store.pageLimit;
  const [limit, setLimit] = useState<string>(defaultValue.toString());

  const handleChange = (value: number | string): void => {
    const numericValue = typeof value === 'string' ? Number.parseInt(value) : value;

    setLimit(value.toString());

    const urlParameters = new URLSearchParams(window.location.search);

    urlParameters.delete('page');
    urlParameters.set('limit', value.toString());

    store.changeLimit(numericValue);

    navigate('?' + urlParameters.toString(), { replace: false, preventScrollReset: true });
  };

  return (
    <Stack direction="row">
      <ButtonGroup size="small">
        <Button
          sx={{
            fontSize: '1.0rem',
            margin: 0,
            padding: 0,
            minHeight: '30px !important',
            minWidth: '30px !important',
          }}
          size="small"
          variant="outlined"
          onClick={(): void => {
            const newValue = Number.parseInt(limit) - 2;
            handleChange(newValue);
          }}
        >
          <span>{t('values.remove')}</span>
        </Button>
        <Box width="3em">
          <TextField
            onChange={(ev): void => {
              ev.preventDefault();
              ev.stopPropagation();
              const newValue = ev.target.value;
              const regex = new RegExp(/^\d*$/);

              if (regex.test(newValue)) {
                setLimit(newValue);
              }
            }}
            onBlur={(ev): void => {
              ev.preventDefault();
              ev.stopPropagation();
              setLimit(defaultValue.toString());
            }}
            onKeyDown={(ev): void => {
              if (ev.key === 'Enter') {
                ev.preventDefault();
                ev.stopPropagation();
                const newValue = Number.parseInt(limit);
                handleChange(newValue);
              }

              if (ev.key === 'Escape') {
                ev.preventDefault();
                ev.stopPropagation();
                setLimit(defaultValue.toString());
              }
            }}
            InputProps={{
              sx: {
                fontSize: '1.0rem',
                margin: 0,
                padding: 0,
                minWidth: '30px !important',
                minHeight: '30px !important',
                textAlign: 'center',
                borderRadius: 0,
              },
            }}
            inputProps={{
              inputMode: 'numeric',
              pattern: '^\\d*$',
              sx: {
                fontSize: '1.0rem',
                margin: 0,
                padding: 0,
                minWidth: '30px !important',
                minHeight: '30px !important',
                textAlign: 'center',
                borderRadius: 0,
              },
            }}
            variant="outlined"
            value={limit}
            size="small"
            margin="none"
            type="text"
          />
        </Box>
        <Button
          sx={{
            fontSize: '1.0rem',
            margin: 0,
            padding: 0,
            minHeight: '30px !important',
            minWidth: '30px !important',
          }}
          size="small"
          variant="outlined"
          onClick={(): void => {
            const newValue = Number.parseInt(limit) + 2;
            handleChange(newValue);
          }}
        >
          <span>{t('values.add')}</span>
        </Button>
      </ButtonGroup>
    </Stack>
  );
});

export default LimitChangerButton;
