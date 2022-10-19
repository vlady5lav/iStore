import 'reflect-metadata';

import { Button, Stack } from '@mui/material';
import { observer } from 'mobx-react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { IoCTypes, useInjection } from 'ioc';
import { CheckoutStore } from 'stores';

interface Properties {
  totalPrice: number;
}

const CheckoutButton = observer((properties: Properties) => {
  const store = useInjection<CheckoutStore>(IoCTypes.checkoutStore);
  const navigate = useNavigate();
  const { t } = useTranslation(['cart']);

  return (
    <Stack direction="column" justifyContent="center">
      <Button
        variant="contained"
        onClick={(): void => {
          store.init();
          navigate('/checkout', { replace: false });
        }}
      >
        {`${t('checkout')} • ${properties.totalPrice}
        ${t('currency', { ns: 'consts' })}`}
      </Button>
    </Stack>
  );
});

export default CheckoutButton;
