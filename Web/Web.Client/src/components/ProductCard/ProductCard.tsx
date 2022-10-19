/* eslint-disable unicorn/no-null */

import 'reflect-metadata';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import InfoIcon from '@mui/icons-material/Info';
import {
  Button,
  Card,
  CardActions,
  CardContent,
  CardMedia,
  Collapse,
  IconButton,
  Stack,
  TextField,
  Tooltip,
  Typography,
  Zoom,
} from '@mui/material';
import { observer } from 'mobx-react';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { BuyButtonProduct } from 'components/BuyButton';
import { Product } from 'models';

interface Properties {
  product: Product;
}

const ProductCard = observer((properties: Properties) => {
  const navigate = useNavigate();
  const { t } = useTranslation(['products']);
  const [expanded, setExpanded] = useState<boolean>(false);

  if (!properties.product) {
    return null;
  }

  const handleExpandClick = (): void => {
    setExpanded(!expanded);
  };

  const { id, name, price, description, pictureUrl, catalogBrand, catalogType } = properties.product;

  return (
    <Card className="productCard" sx={{ width: 250, maxWidth: 250, padding: 1 }}>
      <Stack
        sx={{
          maxHeight: 60,
          padding: 0,
          margin: 0,
        }}
        justifyContent="center"
        alignContent="center"
        textAlign="center"
      >
        <CardContent sx={{ padding: 1, paddingBottom: '8px !important' }}>
          <Stack
            direction="row"
            display="flex"
            flexDirection="row"
            textAlign="center"
            justifyContent="space-between"
            justifyItems="center"
            justifySelf="center"
            alignContent="center"
            alignItems="center"
            alignSelf="center"
          >
            <Stack
              direction="column"
              textAlign="left"
              justifyContent="center"
              justifyItems="center"
              justifySelf="center"
              alignContent="center"
              alignItems="start"
              alignSelf="center"
            >
              <Typography
                sx={{
                  display: '-webkit-box',
                  overflow: 'hidden',
                  textOverflow: 'ellipsis',
                  whiteSpace: 'pre-line',
                  wordBreak: 'break-word',
                  WebkitLineClamp: '1',
                  WebkitBoxOrient: 'vertical',
                }}
              >
                <Tooltip
                  title={catalogBrand.name}
                  placement="top"
                  enterDelay={600}
                  leaveDelay={200}
                  TransitionComponent={Zoom}
                  TransitionProps={{ timeout: 300 }}
                >
                  <strong>{catalogBrand.name}</strong>
                </Tooltip>
              </Typography>
              <Typography
                sx={{
                  display: '-webkit-box',
                  overflow: 'hidden',
                  textOverflow: 'ellipsis',
                  whiteSpace: 'pre-line',
                  wordBreak: 'break-word',
                  WebkitLineClamp: '1',
                  WebkitBoxOrient: 'vertical',
                }}
              >
                <Tooltip
                  title={name}
                  placement="bottom"
                  enterDelay={600}
                  leaveDelay={200}
                  TransitionComponent={Zoom}
                  TransitionProps={{ timeout: 300 }}
                >
                  <strong>{name}</strong>
                </Tooltip>
              </Typography>
            </Stack>
            <Stack
              textAlign="center"
              justifyContent="center"
              justifyItems="center"
              justifySelf="center"
              alignContent="center"
              alignItems="center"
              alignSelf="center"
            >
              <Tooltip
                title={t('tooltips.details')}
                placement="top"
                enterDelay={600}
                leaveDelay={200}
                TransitionComponent={Zoom}
                TransitionProps={{ timeout: 300 }}
              >
                <IconButton
                  className="detailsButton"
                  onClick={(): void => {
                    navigate(`/products/${id}`, { replace: false });
                  }}
                >
                  <InfoIcon />
                </IconButton>
              </Tooltip>
            </Stack>
          </Stack>
        </CardContent>
      </Stack>
      <Stack>
        <CardMedia
          component="img"
          image={pictureUrl}
          alt={`${catalogBrand.name} ${name}`}
          sx={{
            display: 'grid',
            alignContent: 'center',
            alignItems: 'center',
            justifyContent: 'center',
            justifyItems: 'center',
            textAlign: 'center',
            height: 150,
            maxHeight: 150,
            maxWidth: 250,
            padding: 0,
            objectFit: 'contain',
          }}
        />
      </Stack>
      <CardContent sx={{ padding: 0, margin: 0, marginX: 1 }}>
        <Stack direction="row" justifyContent="space-between">
          <Typography textAlign="left" marginRight={6}>
            <strong>{t('properties.type')}:</strong>
          </Typography>
          <Typography
            textAlign="right"
            sx={{
              display: '-webkit-box',
              overflow: 'hidden',
              textOverflow: 'ellipsis',
              whiteSpace: 'pre-line',
              wordBreak: 'break-all',
              WebkitLineClamp: '1',
              WebkitBoxOrient: 'vertical',
            }}
          >
            <Tooltip
              title={catalogType.name}
              placement="top"
              enterDelay={600}
              leaveDelay={200}
              TransitionComponent={Zoom}
              TransitionProps={{ timeout: 300 }}
            >
              <span>{catalogType.name}</span>
            </Tooltip>
          </Typography>
        </Stack>
        <Stack direction="row" justifyContent="space-between">
          <Typography textAlign="left" marginRight={6}>
            <strong>{t('properties.price')}:</strong>
          </Typography>
          <Typography
            textAlign="right"
            sx={{
              display: '-webkit-box',
              overflow: 'hidden',
              textOverflow: 'ellipsis',
              whiteSpace: 'pre-line',
              wordBreak: 'break-all',
              WebkitLineClamp: '1',
              WebkitBoxOrient: 'vertical',
            }}
          >
            <Tooltip
              title={`${price} ${t('consts:currency')}`}
              placement="bottom"
              enterDelay={600}
              leaveDelay={200}
              TransitionComponent={Zoom}
              TransitionProps={{ timeout: 300 }}
            >
              <span>
                {price} {t('consts:currency')}
              </span>
            </Tooltip>
          </Typography>
        </Stack>
      </CardContent>
      <CardActions
        sx={{
          justifyContent: 'space-between',
          height: 40,
          maxHeight: 40,
          padding: 0,
          margin: 1,
          marginBottom: 0,
        }}
      >
        <Stack>
          <BuyButtonProduct productId={id} />
        </Stack>
        <Tooltip
          title={t('tooltips.description')}
          placement="bottom"
          enterDelay={600}
          leaveDelay={200}
          TransitionComponent={Zoom}
          TransitionProps={{ timeout: 300 }}
        >
          <Stack>
            <Button
              className="expandProductButton"
              sx={{ transform: !expanded ? 'rotate(0deg)' : 'rotate(180deg)' }}
              onClick={handleExpandClick}
            >
              <ExpandMoreIcon />
            </Button>
          </Stack>
        </Tooltip>
      </CardActions>
      <Collapse unmountOnExit in={expanded}>
        <CardContent sx={{ padding: 1, paddingBottom: '8px !important' }}>
          <TextField
            color="info"
            fullWidth
            multiline
            focused
            InputProps={{
              sx: {
                fontSize: '0.8rem',
              },
              inputProps: {
                style: {
                  textAlign: 'center',
                },
              },
              readOnly: true,
            }}
            InputLabelProps={{
              sx: { fontSize: '1.0rem', lineHeight: '1.4rem' },
            }}
            id="outlined-multiline-static"
            label={t('properties.description')}
            rows={0}
            value={description}
          />
        </CardContent>
      </Collapse>
    </Card>
  );
});

export default ProductCard;
