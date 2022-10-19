import { useEffect } from 'react';

import { IoCTypes, useInjection } from 'ioc';
import { AuthStore } from 'stores';

function SigninSilentCallback(): JSX.Element {
  const authStore = useInjection<AuthStore>(IoCTypes.authStore);

  useEffect(() => {
    const signinSilentCallback = async (): Promise<void> => {
      await authStore.signinSilentCallback();
    };

    signinSilentCallback().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  }, [authStore]);

  return <></>;
}

export default SigninSilentCallback;
