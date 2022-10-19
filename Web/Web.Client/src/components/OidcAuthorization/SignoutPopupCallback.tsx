import { useEffect } from 'react';

import { IoCTypes, useInjection } from 'ioc';
import { AuthStore } from 'stores';

function SignoutPopupCallback(): JSX.Element {
  const authStore = useInjection<AuthStore>(IoCTypes.authStore);

  useEffect(() => {
    const signoutPopupCallback = async (): Promise<void> => {
      await authStore.signoutPopupCallback();
    };

    signoutPopupCallback().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  }, [authStore]);

  return <></>;
}

export default SignoutPopupCallback;
