import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

import { IoCTypes, useInjection } from 'ioc';
import { AuthStore } from 'stores';

function SignoutCallback(): JSX.Element {
  const authStore = useInjection<AuthStore>(IoCTypes.authStore);
  const navigate = useNavigate();

  useEffect(() => {
    const signoutCallback = async (): Promise<void> => {
      await authStore.signoutCallback(navigate);
    };

    signoutCallback().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  }, [authStore, navigate]);

  return <></>;
}

export default SignoutCallback;
