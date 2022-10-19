import 'reflect-metadata';

import { injectable } from 'inversify';
import { Log, SessionStatus, SignoutResponse, User, UserManager } from 'oidc-client-ts';

import { OidcConfig } from 'utils';

export interface AuthenticationService {
  clearStaleState: () => Promise<void>;
  getUser: () => Promise<User | null>;
  getUserLocalStorage: () => User;
  getUserSessionStorage: () => User;
  parseJwt: (token: string) => unknown;
  querySessionStatus: () => Promise<SessionStatus | null>;
  revokeTokens: () => Promise<void>;
  signinCallback: () => Promise<void | User>;
  signinPopup: () => Promise<User>;
  signinPopupCallback: () => Promise<void>;
  signinRedirect: () => Promise<void>;
  signinRedirectCallback: () => Promise<User>;
  signinSilent: () => Promise<User | null>;
  signinSilentCallback: () => Promise<void>;
  signoutCallback: () => Promise<void>;
  signoutPopup: () => Promise<void>;
  signoutPopupCallback: () => Promise<void>;
  signoutRedirect: () => Promise<void>;
  signoutRedirectCallback: () => Promise<SignoutResponse>;
  startSilentRenew: () => void;
  stopSilentRenew: () => void;
  storeUser: (user: User | null) => Promise<void>;
}

@injectable()
export default class DefaultAuthenticationService implements AuthenticationService {
  private readonly userManager: UserManager;

  constructor() {
    this.userManager = new UserManager(OidcConfig);
    this.configUserManager();
  }

  public configUserManager = (): void => {
    Log.setLogger(console);
    Log.setLevel(Log.INFO);

    this.userManager.events.addSilentRenewError((error) => {
      console.log(`Silent renew error:\n${error.name}\n${error.message}\n${error.cause}`);
    });

    this.userManager.events.addUserSignedIn(() => {
      console.log('User logged in to the token server!');
    });

    this.userManager.events.addUserSignedOut(() => {
      console.log('User logged out of the token server!');
    });

    this.userManager.events.addUserLoaded((user) => {
      console.log(`User loaded: ${user.profile.sub}!`);

      /*
      this.userManager
        .getUser()
        .then(() => {
          console.log(`Method 'getUser' loaded user '${user.profile.sub}' after userLoaded event fired!`);
        })
        .catch((error) => {
          console.log(`Get user error:\n${error.name}\n${error.message}\n${error.cause}`);
        });
        */
    });

    this.userManager.events.addUserUnloaded(() => {
      console.log('User unloaded!');
    });

    this.userManager.events.addAccessTokenExpiring(() => {
      console.log('Token expiring!');

      this.userManager
        .signinSilent()
        .then((user) => {
          console.log(`Silent renew for user '${user?.profile.sub}' was successful!`);
        })
        .catch((error) => {
          if (error instanceof Error) {
            console.log(`Silent renew error:\n${error.name}\n${error.message}\n${error.cause}`);
          } else {
            console.log(error);
          }
        });
    });

    this.userManager.events.addAccessTokenExpired(() => {
      console.log('Token expired!');
    });
  };

  public clearStaleState = async (): Promise<void> => {
    await this.userManager.clearStaleState().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  };

  public getUser = async (): Promise<User | null> => {
    return await this.userManager.getUser();
  };

  public getUserLocalStorage = (): User => {
    const oidcUser: User = JSON.parse(
      String(
        localStorage.getItem(`oidc.user:${process.env.REACT_APP_AUTH_URL}:${process.env.REACT_APP_IDENTITY_CLIENT_ID}`)
      )
    ) as User;

    return oidcUser;
  };

  public getUserSessionStorage = (): User => {
    const oidcUser: User = JSON.parse(
      String(
        sessionStorage.getItem(
          `oidc.user:${process.env.REACT_APP_AUTH_URL}:${process.env.REACT_APP_IDENTITY_CLIENT_ID}`
        )
      )
    ) as User;

    return oidcUser;
  };

  public parseJwt = (token: string): unknown => {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace('-', '+').replace('_', '/');

    return JSON.parse(window.atob(base64));
  };

  public querySessionStatus = async (): Promise<SessionStatus | null> => {
    return await this.userManager.querySessionStatus();
  };

  public revokeTokens = async (): Promise<void> => {
    return await this.userManager.revokeTokens().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  };

  public signinCallback = async (): Promise<void | User> => {
    return await this.userManager.signinCallback().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  };

  public signinPopup = async (): Promise<User> => {
    return await this.userManager.signinPopup();
  };

  public signinPopupCallback = async (): Promise<void> => {
    return await this.userManager.signinPopupCallback().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  };

  public signinRedirect = async (): Promise<void> => {
    await this.userManager.signinRedirect().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  };

  public signinRedirectCallback = async (): Promise<User> => {
    return await this.userManager.signinRedirectCallback();
  };

  public signinSilent = async (): Promise<User | null> => {
    return await this.userManager.signinSilent();
  };

  public signinSilentCallback = async (): Promise<void> => {
    return await this.userManager.signinSilentCallback().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  };

  public signoutCallback = async (): Promise<void> => {
    await this.userManager.signoutCallback().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  };

  public signoutPopup = async (): Promise<void> => {
    await this.userManager.signoutPopup().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  };

  public signoutPopupCallback = async (): Promise<void> => {
    await this.userManager.signoutPopupCallback().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  };

  public signoutRedirect = async (): Promise<void> => {
    await this.userManager.signoutRedirect().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  };

  public signoutRedirectCallback = async (): Promise<SignoutResponse> => {
    return await this.userManager.signoutRedirectCallback();
  };

  public startSilentRenew = (): void => {
    this.userManager.startSilentRenew();
  };

  public stopSilentRenew = (): void => {
    this.userManager.stopSilentRenew();
  };

  public storeUser = async (user: User | null): Promise<void> => {
    await this.userManager.storeUser(user).catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  };
}
