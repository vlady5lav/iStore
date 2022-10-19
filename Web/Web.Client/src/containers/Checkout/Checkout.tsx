import 'reflect-metadata';

import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Container from '@mui/material/Container';
import Paper from '@mui/material/Paper';
import Step from '@mui/material/Step';
import StepLabel from '@mui/material/StepLabel';
import Stepper from '@mui/material/Stepper';
import Typography from '@mui/material/Typography';
import { useInjection } from 'inversify-react';
import { observer } from 'mobx-react';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { AddressForm, PaymentForm, Review } from 'components/Checkout';
import { IoCTypes } from 'ioc';
import { CartStore, CheckoutStore } from 'stores';

const order = {
  number: Math.round(Math.random() * Math.pow(10, 9)),
};

const getStepContent = (step: number): JSX.Element => {
  switch (step) {
    case 0: {
      return <AddressForm />;
    }

    case 1: {
      return <PaymentForm />;
    }

    case 2: {
      return <Review />;
    }

    default: {
      throw new Error('Unknown step');
    }
  }
};

const Checkout = observer((): JSX.Element => {
  const store = useInjection<CheckoutStore>(IoCTypes.checkoutStore);
  const cartStore = useInjection<CartStore>(IoCTypes.cartStore);
  const { t } = useTranslation(['checkout']);

  const steps = [t('steps.shipping'), t('steps.payment'), t('steps.review')];

  const [activeStep, setActiveStep] = useState(0);

  const handleNext = (): void => {
    setActiveStep(activeStep + 1);
  };

  const handleBack = (): void => {
    setActiveStep(activeStep - 1);
  };

  return (
    <Container component="main" maxWidth="sm">
      <Paper variant="outlined" sx={{ my: { xs: 3, md: 6 }, p: { xs: 2, md: 3 } }}>
        <Typography component="h1" variant="h4" align="center">
          <span>{t('checkout.checkout')}</span>
        </Typography>
        <Stepper activeStep={activeStep} sx={{ pt: 3, pb: 5 }}>
          {steps.map((label) => (
            <Step key={label}>
              <StepLabel>{label}</StepLabel>
            </Step>
          ))}
        </Stepper>
        <>
          {activeStep === steps.length ? (
            <>
              <Typography variant="h5" gutterBottom whiteSpace="pre-line">
                <span>{t('checkout.thanks')}</span>
              </Typography>
              <Typography variant="subtitle1" whiteSpace="pre-line">
                <span>{t('checkout.order_confirmed', { order: order, interpolation: { escapeValue: false } })}</span>
              </Typography>
            </>
          ) : (
            <>
              {getStepContent(activeStep)}
              <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                {activeStep !== 0 && (
                  <Button onClick={handleBack} sx={{ mt: 3, ml: 1 }}>
                    <span>{t('checkout.back')}</span>
                  </Button>
                )}
                <Button
                  variant="contained"
                  onClick={
                    activeStep === steps.length - 1
                      ? async (): Promise<void> => {
                          store.init();
                          await cartStore.deleteCart();
                          handleNext();
                        }
                      : handleNext
                  }
                  sx={{ mt: 3, ml: 1 }}
                >
                  {activeStep === steps.length - 1 ? t('checkout.confirm') : t('checkout.next')}
                </Button>
              </Box>
            </>
          )}
        </>
      </Paper>
    </Container>
  );
});

export default Checkout;
