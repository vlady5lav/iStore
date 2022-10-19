import 'reflect-metadata';

import DescriptionIcon from '@mui/icons-material/Description';
import LoginIcon from '@mui/icons-material/Login';
import LogoutIcon from '@mui/icons-material/Logout';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import { Avatar, Badge, Button, IconButton, ListItemIcon, Menu, MenuItem, Paper, Stack, Tooltip } from '@mui/material';
import { bindMenu, bindTrigger, usePopupState } from 'material-ui-popup-state/hooks';
import { observer } from 'mobx-react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import { IoCTypes, useInjection } from 'ioc';
import { AuthStore, CartStore } from 'stores';

const Header = observer((): JSX.Element => {
  const navigate = useNavigate();
  const authStore = useInjection<AuthStore>(IoCTypes.authStore);
  const cartStore = useInjection<CartStore>(IoCTypes.cartStore);
  const { t } = useTranslation(['header']);
  const popupState = usePopupState({
    variant: 'popover',
    popupId: 'accountMenu',
  });

  return (
    <Paper
      elevation={1}
      sx={{
        borderRadius: '0px',
        display: 'grid',
        justifyContent: 'center',
        justifyItems: 'center',
        alignContent: 'center',
        alignItems: 'center',
        px: 0,
        pt: 2,
        pb: 1,
        mb: 'auto',
      }}
    >
      <Stack
        direction="row"
        spacing={{ xs: 1, sm: 2, md: 4 }}
        justifyContent="center"
        justifyItems="center"
        alignContent="center"
        alignItems="center"
      >
        <Button
          sx={{ height: 45, width: 'auto' }}
          className="productsButton"
          color="warning"
          endIcon={<DescriptionIcon />}
          variant="contained"
          onClick={(): void => {
            navigate('/products', { replace: false });
          }}
        >
          {t('products')}
        </Button>
        <Badge color="secondary" badgeContent={cartStore.cart?.totalCount ?? undefined}>
          <Button
            sx={{ height: 45, width: 'auto' }}
            className="cartButton"
            color="warning"
            endIcon={<ShoppingCartIcon />}
            variant="contained"
            onClick={(): void => {
              navigate('/cart', { replace: false });
            }}
          >
            {t('cart')}
          </Button>
        </Badge>
        {!authStore.user && (
          <Tooltip title={t('login')}>
            <IconButton
              className="signinButton"
              onClick={async (): Promise<void> => {
                await authStore.signinRedirect();
              }}
            >
              <Avatar variant="circular" sx={{ width: 45, height: 45, bgcolor: 'darkcyan' }}>
                <LoginIcon />
              </Avatar>
            </IconButton>
          </Tooltip>
        )}
        {authStore.user && (
          <>
            <Tooltip title={t('account')}>
              <IconButton {...bindTrigger(popupState)}>
                <Avatar variant="circular" sx={{ width: 45, height: 45, bgcolor: 'darkcyan' }}>
                  {authStore.user?.profile?.given_name?.slice(0, 1)}
                </Avatar>
              </IconButton>
            </Tooltip>
            <Menu {...bindMenu(popupState)}>
              <MenuItem
                onClick={async (): Promise<void> => {
                  popupState.close();
                  await authStore.signoutRedirect();
                }}
              >
                <ListItemIcon>
                  <LogoutIcon />
                </ListItemIcon>
                {t('logout')}
              </MenuItem>
            </Menu>
          </>
        )}
      </Stack>
    </Paper>
  );
});

export default Header;
