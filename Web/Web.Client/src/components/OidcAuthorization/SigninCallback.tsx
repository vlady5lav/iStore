import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

import { IoCTypes, useInjection } from 'ioc';
import { AuthStore } from 'stores';

function SigninCallback(): JSX.Element {
  const authStore = useInjection<AuthStore>(IoCTypes.authStore);
  const navigate = useNavigate();

  useEffect(() => {
    const signinCallback = async (): Promise<void> => {
      await authStore.signinCallback(navigate);
    };

    signinCallback().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  }, [authStore, navigate]);

  return <></>;
}

export default SigninCallback;
