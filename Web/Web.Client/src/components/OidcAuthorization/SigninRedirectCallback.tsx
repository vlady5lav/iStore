import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

import { IoCTypes, useInjection } from 'ioc';
import { AuthStore } from 'stores';

function SigninRedirectCallback(): JSX.Element {
  const authStore = useInjection<AuthStore>(IoCTypes.authStore);
  const navigate = useNavigate();

  useEffect(() => {
    const signinRedirectCallback = async (): Promise<void> => {
      await authStore.signinRedirectCallback(navigate);
    };

    signinRedirectCallback().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  }, [authStore, navigate]);

  return <></>;
}

export default SigninRedirectCallback;
