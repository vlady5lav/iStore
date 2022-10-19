import 'reflect-metadata';

import Checkbox from '@mui/material/Checkbox';
import FormControlLabel from '@mui/material/FormControlLabel';
import Grid from '@mui/material/Grid';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import { useInjection } from 'inversify-react';
import { observer } from 'mobx-react';
import { useTranslation } from 'react-i18next';

import { IoCTypes } from 'ioc';
import { CheckoutStore } from 'stores';

const AddressForm = observer((): JSX.Element => {
  const store = useInjection<CheckoutStore>(IoCTypes.checkoutStore);
  const { t } = useTranslation('checkout');

  return (
    <>
      <Typography variant="h6" gutterBottom>
        {t('address_form.shipping_address')}
      </Typography>
      <Grid container spacing={3}>
        <Grid item xs={12} sm={6}>
          <TextField
            required
            id="firstName"
            name="firstName"
            label={t('address_form.first_name')}
            fullWidth
            autoComplete="given-name"
            variant="standard"
            value={store.firstName}
            onChange={(event): void => store.changeFirstName(event.target.value)}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            required
            id="lastName"
            name="lastName"
            label={t('address_form.last_name')}
            fullWidth
            autoComplete="family-name"
            variant="standard"
            value={store.lastName}
            onChange={(event): void => store.changeLastName(event.target.value)}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            required
            id="address1"
            name="address1"
            label={t('address_form.address_line_1')}
            fullWidth
            autoComplete="shipping address-line1"
            variant="standard"
            value={store.address1}
            onChange={(event): void => store.changeAddress1(event.target.value)}
          />
        </Grid>
        <Grid item xs={12}>
          <TextField
            id="address2"
            name="address2"
            label={t('address_form.address_line_2')}
            fullWidth
            autoComplete="shipping address-line2"
            variant="standard"
            value={store.address2}
            onChange={(event): void => store.changeAddress2(event.target.value)}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            required
            id="city"
            name="city"
            label={t('address_form.city')}
            fullWidth
            autoComplete="shipping address-level2"
            variant="standard"
            value={store.city}
            onChange={(event): void => store.changeCity(event.target.value)}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            id="state"
            name="state"
            label={t('address_form.state')}
            fullWidth
            variant="standard"
            value={store.state}
            onChange={(event): void => store.changeState(event.target.value)}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            required
            id="zip"
            name="zip"
            label={t('address_form.zip')}
            fullWidth
            autoComplete="shipping postal-code"
            variant="standard"
            value={store.zip}
            onChange={(event): void => store.changeZip(event.target.value)}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <TextField
            required
            id="country"
            name="country"
            label={t('address_form.country')}
            fullWidth
            autoComplete="shipping country"
            variant="standard"
            value={store.country}
            onChange={(event): void => store.changeCountry(event.target.value)}
          />
        </Grid>
        <Grid item xs={12}>
          <FormControlLabel
            control={<Checkbox color="secondary" name="saveAddress" value="yes" />}
            label={t('address_form.use_address_for_payment')}
          />
        </Grid>
      </Grid>
    </>
  );
});

export default AddressForm;
