import 'reflect-metadata';

import { observer } from 'mobx-react';
import { useEffect } from 'react';
import { Outlet } from 'react-router-dom';

import { LoadingSpinner } from 'components/LoadingSpinner';
import { IoCTypes, useInjection } from 'ioc';
import { AuthStore } from 'stores';

const AuthorizedOutlet = observer(() => {
  const authStore = useInjection<AuthStore>(IoCTypes.authStore);

  useEffect(() => {
    const getAuthenticationStatus = async (): Promise<void> => {
      if (!authStore.user) {
        await authStore.signinRedirect();
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

  return authStore.user ? <Outlet /> : <LoadingSpinner />;
});

export default AuthorizedOutlet;
