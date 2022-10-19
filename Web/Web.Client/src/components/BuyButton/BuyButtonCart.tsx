import 'reflect-metadata';

import DeleteForeverIcon from '@mui/icons-material/DeleteForever';
import { Box, Button, ButtonGroup, Stack, TextField } from '@mui/material';
import { observer } from 'mobx-react';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { IoCTypes, useInjection } from 'ioc';
import { CartStore } from 'stores';

interface Properties {
  productId: number;
  productCount: number;
}

const BuyButtonCart = observer(({ productId, productCount }: Properties): JSX.Element => {
  const store = useInjection<CartStore>(IoCTypes.cartStore);
  const { t } = useTranslation(['cart']);

  const [count, setCount] = useState<string>(productCount.toString());

  return (
    <Stack direction="row">
      <ButtonGroup size="small" sx={{ marginRight: 1 }}>
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
          onClick={async (): Promise<void> => {
            await store.clearItem(productId);
          }}
        >
          <DeleteForeverIcon />
        </Button>
      </ButtonGroup>
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
          onClick={async (): Promise<void> => {
            await store.removeItem(productId);
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
                setCount(newValue);
              }
            }}
            onBlur={async (ev): Promise<void> => {
              ev.preventDefault();
              ev.stopPropagation();
              await store.setCount(productId, count);
            }}
            onKeyDown={async (ev): Promise<void> => {
              if (ev.key === 'Enter') {
                ev.preventDefault();
                ev.stopPropagation();
                await store.setCount(productId, count);
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
            value={count}
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
          onClick={async (): Promise<void> => {
            await store.addItem(productId);
          }}
        >
          <span>{t('values.add')}</span>
        </Button>
      </ButtonGroup>
    </Stack>
  );
});

export default BuyButtonCart;
