/* eslint-disable unicorn/no-null */

import 'reflect-metadata';

import { Card, CardActions, CardContent, CardMedia, Stack, TextField, Typography } from '@mui/material';
import { observer } from 'mobx-react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { BuyButtonProduct } from 'components/BuyButton';
import { Product } from 'models';

interface Properties {
  product: Product;
}

const ProductDetails = observer((properties: Properties) => {
  const navigate = useNavigate();
  const { t } = useTranslation(['products']);

  if (!properties.product) {
    return null;
  }

  const { id, name, price, warranty, description, pictureUrl, catalogBrand, catalogType } = properties.product;

  return (
    <Stack
      className="productDetails"
      sx={{
        display: 'grid',
        height: 'fit-content',
        width: 'fit-content',
        maxWidth: '800px',
        justifyContent: 'center',
        justifyItems: 'center',
        alignContent: 'center',
        alignItems: 'center',
        textAlign: 'center',
        padding: 0,
        margin: 0,
      }}
    >
      <Card sx={{ padding: 1 }}>
        <Stack
          sx={{
            display: 'grid',
            justifyContent: 'center',
            justifyItems: 'center',
            alignContent: 'center',
            alignItems: 'center',
            textAlign: 'center',
          }}
        >
          <CardMedia
            sx={{
              display: 'grid',
              justifyContent: 'center',
              justifyItems: 'center',
              alignContent: 'center',
              alignItems: 'center',
              textAlign: 'center',
              width: 'auto',
              height: 'auto',
              maxHeight: '50vh',
              maxWidth: '70vw',
              objectFit: 'contain',
            }}
            component="img"
            image={pictureUrl}
            alt={`${catalogBrand.name} ${name}`}
            onClick={(): void => {
              navigate(`/products/${id}`, { replace: false });
            }}
          />
        </Stack>
        <CardContent sx={{ padding: 0.5, margin: 0.5, marginX: 2, textAlign: 'left' }}>
          <Stack direction="row" justifyContent="space-between">
            <Typography textAlign="left" marginRight={6}>
              <strong>{t('properties.type')}:</strong>
            </Typography>
            <Typography textAlign="right">{catalogType.name}</Typography>
          </Stack>
          <Stack direction="row" justifyContent="space-between">
            <Typography textAlign="left" marginRight={6}>
              <strong>{t('properties.brand')}:</strong>
            </Typography>
            <Typography textAlign="right">{catalogBrand.name}</Typography>
          </Stack>
          <Stack direction="row" justifyContent="space-between">
            <Typography textAlign="left" marginRight={6}>
              <strong>{t('properties.name')}:</strong>
            </Typography>
            <Typography textAlign="right">{name}</Typography>
          </Stack>
          <Stack direction="row" justifyContent="space-between">
            <Typography textAlign="left" marginRight={6}>
              <strong>{t('properties.warranty')}:</strong>
            </Typography>
            <Typography textAlign="right">{warranty}</Typography>
          </Stack>
          <Stack direction="row" justifyContent="space-between">
            <Typography textAlign="left" marginRight={6}>
              <strong>{t('properties.price')}:</strong>
            </Typography>
            <Typography textAlign="right">
              {price} {t('consts:currency')}
            </Typography>
          </Stack>
        </CardContent>
        <CardContent sx={{ marginTop: 2, padding: 1, paddingBottom: '8px !important' }}>
          <TextField
            color="info"
            fullWidth
            multiline
            focused
            sx={{ fontSize: '1.0rem' }}
            InputProps={{
              sx: {
                fontSize: '1.0rem',
              },
              inputProps: {
                style: {
                  textAlign: 'center',
                },
              },
              readOnly: true,
            }}
            InputLabelProps={{
              sx: { fontSize: '1.1rem', lineHeight: '1.4rem' },
            }}
            id="outlined-multiline-static"
            label={t('properties.description')}
            rows={0}
            value={description}
          />
        </CardContent>
        <CardActions sx={{ justifyContent: 'center', padding: 0, margin: 1 }}>
          <Stack>
            <BuyButtonProduct productId={id} />
          </Stack>
        </CardActions>
      </Card>
    </Stack>
  );
});

export default ProductDetails;
