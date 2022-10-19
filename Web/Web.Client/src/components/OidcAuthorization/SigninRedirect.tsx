import { useEffect } from 'react';

import { IoCTypes, useInjection } from 'ioc';
import { AuthStore } from 'stores';

function SigninRedirect(): JSX.Element {
  const authStore = useInjection<AuthStore>(IoCTypes.authStore);

  useEffect(() => {
    const signinRedirect = async (): Promise<void> => {
      await authStore.signinRedirect();
    };

    signinRedirect().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  }, [authStore, authStore.user]);

  return <></>;
}

export default SigninRedirect;
