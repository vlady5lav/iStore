import 'reflect-metadata';

import Grid from '@mui/material/Grid';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import Typography from '@mui/material/Typography';
import { useInjection } from 'inversify-react';
import { observer } from 'mobx-react';
import { Fragment } from 'react';
import { useTranslation } from 'react-i18next';

import { IoCTypes } from 'ioc';
import { CartStore, CheckoutStore } from 'stores';

const Review = observer((): JSX.Element => {
  const store = useInjection<CheckoutStore>(IoCTypes.checkoutStore);
  const cartStore = useInjection<CartStore>(IoCTypes.cartStore);
  const { t } = useTranslation('checkout');

  const products = cartStore.cart.items;
  const addresses = [store.zip, store.country, store.state, store.city, store.address1, store.address2];
  const payments = [
    { name: t('review.card_holder'), detail: store.cardName },
    { name: t('review.card_number'), detail: store.cardNumber },
    { name: t('review.expiry_date'), detail: store.expDate },
  ];

  return (
    <>
      <Typography variant="h6" gutterBottom>
        <span>{t('review.order_summary')}</span>
      </Typography>
      <List disablePadding>
        {products.map((product) => (
          <ListItem key={product.name} sx={{ py: 1, px: 0 }}>
            <ListItemText primary={`${product.brand} ${product.name}`} secondary={product.type} />
            <Typography variant="body2">{product.price}</Typography>
          </ListItem>
        ))}
        <ListItem sx={{ py: 1, px: 0 }}>
          <ListItemText primary={t('review.total')} />
          <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
            {cartStore.cart.totalPrice}
          </Typography>
        </ListItem>
      </List>
      <Grid container spacing={2}>
        <Grid item xs={12} sm={6}>
          <Typography variant="h6" gutterBottom sx={{ mt: 2 }}>
            <span>{t('review.shipping')}</span>
          </Typography>
          <Typography gutterBottom>{`${store.firstName} ${store.lastName}`}</Typography>
          <Typography gutterBottom>
            {addresses
              .filter((val, idx, arr) => {
                return val !== '';
              })
              .join(', ')}
          </Typography>
        </Grid>
        <Grid item container direction="column" xs={12} sm={6}>
          <Typography variant="h6" gutterBottom sx={{ mt: 2 }}>
            <span>{t('review.payment_details')}</span>
          </Typography>
          <Grid container>
            {payments.map((payment) => (
              <Fragment key={payment.name}>
                <Grid item xs={6}>
                  <Typography gutterBottom>{payment.name}</Typography>
                </Grid>
                <Grid item xs={6}>
                  <Typography gutterBottom>{payment.detail}</Typography>
                </Grid>
              </Fragment>
            ))}
          </Grid>
        </Grid>
      </Grid>
    </>
  );
});

export default Review;
