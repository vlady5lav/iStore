import 'reflect-metadata';

import { Card, CardContent, CardMedia, Stack, Typography } from '@mui/material';
import { observer } from 'mobx-react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { BuyButtonCart } from 'components/BuyButton';
import { CartItem } from 'models';

interface Properties {
  item: CartItem;
}

const CartCard = observer((properties: Properties) => {
  const navigate = useNavigate();
  const { t } = useTranslation('cart');
  const { id, brand, type, name, picture, count, totalPrice } = properties.item;

  return (
    <Card
      className="productCard"
      sx={{
        width: '95vw',
        maxWidth: '800px',
        padding: 1.5,
        margin: 0,
        textAlign: 'center',
      }}
    >
      <Stack direction="row" justifyContent="space-between">
        <Stack justifyContent="center">
          <CardMedia
            component="img"
            image={picture}
            alt={`${name}`}
            sx={{
              display: 'grid',
              alignContent: 'center',
              justifyContent: 'left',
              margin: 0,
              marginRight: 1,
              padding: 0,
              height: 'auto',
              width: 'auto',
              maxHeight: 115,
              maxWidth: 115,
              objectFit: 'contain',
            }}
            onClick={(): void => {
              navigate(`/products/${id}`, { replace: false });
            }}
          />
        </Stack>
        <Stack>
          <Stack display="grid" justifyContent="right">
            <CardContent
              sx={{
                display: 'grid',
                alignContent: 'right',
                justifyContent: 'right',
                textAlign: 'right',
                margin: 0,
                marginLeft: 1,
                padding: 0,
                paddingBottom: '0px !important',
                paddingRight: 2,
                width: 'fit-content',
                height: 'fit-content',
              }}
            >
              <Typography>{`${type} ${brand}`}</Typography>
              <Typography>{`${name}`}</Typography>
            </CardContent>
          </Stack>
          <Stack direction="row" justifyContent="right" marginTop={1}>
            <Stack justifyContent="center">
              <CardContent
                sx={{
                  display: 'grid',
                  alignContent: 'center',
                  justifyContent: 'right',
                  textAlign: 'right',
                  margin: 0,
                  padding: 0,
                  paddingBottom: '0px !important',
                }}
              >
                <BuyButtonCart productCount={count} productId={id} />
              </CardContent>
            </Stack>
            <Stack justifyContent="center">
              <CardContent
                sx={{
                  display: 'grid',
                  width: '5.5em',
                  alignContent: 'center',
                  justifyContent: 'right',
                  textAlign: 'right',
                  margin: 0,
                  padding: 0,
                  paddingBottom: '0px !important',
                  paddingRight: 2,
                }}
              >
                <Typography>
                  <strong>{totalPrice}</strong>
                </Typography>
                <Typography>
                  <strong>{t('currency', { ns: 'consts' })}</strong>
                </Typography>
              </CardContent>
            </Stack>
          </Stack>
        </Stack>
      </Stack>
    </Card>
  );
});

export default CartCard;
