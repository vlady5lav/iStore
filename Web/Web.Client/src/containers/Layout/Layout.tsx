import 'reflect-metadata';

import { Box } from '@mui/material';
import { observer } from 'mobx-react';
import { useEffect } from 'react';
import { Outlet } from 'react-router-dom';

import { Footer } from 'components/Footer';
import { Header } from 'components/Header';
import { IoCTypes, useInjection } from 'ioc';
import { AuthStore, CartStore } from 'stores';

const Layout = observer(() => {
  const authStore = useInjection<AuthStore>(IoCTypes.authStore);
  const cartStore = useInjection<CartStore>(IoCTypes.cartStore);

  useEffect(() => {
    const getAuthenticationStatus = async (): Promise<void> => {
      await authStore.getUser();

      if (!authStore.user) {
        await authStore.signinSilent();
      }
    };

    getAuthenticationStatus().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  }, [authStore]);

  useEffect(() => {
    const getCart = async (): Promise<void> => {
      await cartStore.getCart();
    };

    getCart().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  }, [authStore.user, cartStore]);

  return (
    <Box id="layout" className="layout" display="flex" flexDirection="column" minWidth="100%" minHeight="100vh">
      <Box id="header" className="header" position="relative" top={0} left={0} right={0} minWidth="100%">
        <Header />
      </Box>
      <Box
        id="main"
        className="main"
        display="flex"
        position="relative"
        left={0}
        right={0}
        minWidth="100%"
        minHeight="70vh"
      >
        <Outlet />
      </Box>
      <Box id="footer" className="footer" position="relative" bottom={0} left={0} right={0} minWidth="100%" mt="auto">
        <Footer />
      </Box>
    </Box>
  );
});

export default Layout;
