import 'reflect-metadata';

import AddShoppingCartIcon from '@mui/icons-material/AddShoppingCart';
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';
import { Box, Button, ButtonGroup, IconButton, Stack, TextField, Tooltip, Zoom } from '@mui/material';
import { observer } from 'mobx-react';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { IoCTypes, useInjection } from 'ioc';
import { CartStore } from 'stores';

interface Properties {
  productId: number;
}

const BuyButtonProduct = observer(({ productId }: Properties): JSX.Element => {
  const store = useInjection<CartStore>(IoCTypes.cartStore);
  const { t } = useTranslation(['products']);

  const item = store.cart.items.find((p) => p.id === productId);
  const itemCount = item ? item.count : 0;

  const [count, setCount] = useState<string>(itemCount.toString());

  useEffect(() => {
    setCount(itemCount.toString());
  }, [itemCount]);

  return (
    <Stack direction="row">
      {itemCount <= 0 && (
        <Tooltip
          title={t('tooltips.cart_add')}
          placement="bottom"
          enterDelay={600}
          leaveDelay={200}
          TransitionComponent={Zoom}
          TransitionProps={{ timeout: 300 }}
        >
          <Stack>
            <IconButton
              onClick={async (): Promise<void> => {
                await store.addItem(productId);
              }}
            >
              <AddShoppingCartIcon />
            </IconButton>
          </Stack>
        </Tooltip>
      )}
      {itemCount > 0 && (
        <>
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
                onBlur={(ev): void => {
                  ev.preventDefault();
                  ev.stopPropagation();
                  setCount(itemCount.toString());
                }}
                onKeyDown={async (ev): Promise<void> => {
                  if (ev.key === 'Enter') {
                    ev.preventDefault();
                    ev.stopPropagation();
                    await store.setCount(productId, count);
                  }

                  if (ev.key === 'Escape') {
                    ev.preventDefault();
                    ev.stopPropagation();
                    setCount(itemCount.toString());
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
        </>
      )}
    </Stack>
  );
});

export default BuyButtonProduct;
