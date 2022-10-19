import 'reflect-metadata';

import { Grid, Stack, Typography } from '@mui/material';
import { observer } from 'mobx-react';
import { useTranslation } from 'react-i18next';

import { CartCard } from 'components/CartCard';
import { CheckoutButton } from 'components/CheckoutButton';
import { LoadingSpinner } from 'components/LoadingSpinner';
import { IoCTypes, useInjection } from 'ioc';
import { CartStore } from 'stores';

const Cart = observer(() => {
  const store = useInjection<CartStore>(IoCTypes.cartStore);
  const { t } = useTranslation(['cart']);

  return (
    <>
      {store.isLoading ? (
        <LoadingSpinner />
      ) : (
        <Grid key={Math.random() * 12_345} container justifyContent="center" marginY={4} marginX={1}>
          <Grid key={Math.random() * 12_345} container justifyContent="center">
            {store.cart.items.length > 0 ? (
              <Stack direction="column">
                {store.cart.items.map((item) => (
                  <Grid key={Math.random() * 12_345} item justifyContent="center" marginBottom={4}>
                    <CartCard item={item} />
                  </Grid>
                ))}
                <Grid key={Math.random() * 12_345} item justifyContent="center">
                  <CheckoutButton totalPrice={store.cart.totalPrice} />
                </Grid>
              </Stack>
            ) : (
              <Grid
                display="flex"
                key={Math.random() * 12_345}
                item
                textAlign="center"
                justifyContent="center"
                justifyItems="center"
                justifySelf="center"
                alignContent="center"
                alignItems="center"
                alignSelf="center"
                minHeight="100%"
                minWidth="100%"
              >
                <Typography whiteSpace="pre-line">{t('placeholder.empty')}</Typography>
              </Grid>
            )}
          </Grid>
        </Grid>
      )}
    </>
  );
});

export default Cart;
