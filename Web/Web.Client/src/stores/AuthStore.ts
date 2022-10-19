import 'reflect-metadata';

import { inject, injectable } from 'inversify';
import { makeAutoObservable } from 'mobx';
import { User } from 'oidc-client-ts';

import { IoCTypes } from 'ioc';
import { NavigateFunction } from 'react-router-dom';
import type { AuthenticationService } from 'services/AuthenticationService';

@injectable()
export default class AuthStore {
  @inject(IoCTypes.authenticationService)
  private readonly authenticationService!: AuthenticationService;

  user: User | null = null;

  constructor() {
    this.user = null;
    makeAutoObservable(this);
  }

  public getSavedLocation = (): string | null => {
    return localStorage.getItem('redirectUri');
  };

  public getUser = async (): Promise<void> => {
    const userResponse = await this.authenticationService.getUser();

    if (userResponse) {
      this.user = userResponse;
    }
  };

  public removeRedirectLocation = (): void => {
    localStorage.removeItem('redirectUri');
  };

  public replaceLocation = (navigate: NavigateFunction): void => {
    const destination = localStorage.getItem('redirectUri') || '/';
    navigate(destination, { replace: false, preventScrollReset: true });
  };

  public saveLocation = (location?: string): void => {
    if (location) {
      localStorage.setItem('redirectUri', location);
    } else if (
      !window.location.pathname.includes('/checkout') &&
      !window.location.pathname.includes('/signin') &&
      !window.location.pathname.includes('/signout')
    ) {
      localStorage.setItem('redirectUri', window.location.pathname + window.location.search);
    } else if (window.location.pathname.includes('/checkout')) {
      localStorage.setItem('redirectUri', '/cart');
    } else {
      localStorage.setItem('redirectUri', '/');
    }
  };

  public signinCallback = async (navigate: NavigateFunction): Promise<void> => {
    const signinResponse = await this.authenticationService.signinCallback();

    if (signinResponse) {
      this.user = signinResponse;
    } else {
      await this.getUser();
    }

    if (this.user) {
      this.authenticationService.startSilentRenew();
    } else {
      await this.authenticationService.clearStaleState();
    }

    if (this.getSavedLocation()) {
      this.replaceLocation(navigate);
      this.removeRedirectLocation();
    }
  };

  public signinPopup = async (location?: string): Promise<void> => {
    this.authenticationService.stopSilentRenew();
    await this.authenticationService.clearStaleState();
    const signinResponse = await this.authenticationService.signinPopup();

    if (signinResponse) {
      this.user = signinResponse;
      this.authenticationService.startSilentRenew();
    } else {
      await this.authenticationService.clearStaleState();
    }
  };

  public signinPopupCallback = async (): Promise<void> => {
    await this.authenticationService.signinPopupCallback();
  };

  public signinRedirect = async (location?: string): Promise<void> => {
    this.saveLocation(location);
    this.authenticationService.stopSilentRenew();
    await this.authenticationService.clearStaleState();
    await this.authenticationService.signinRedirect();
  };

  public signinRedirectCallback = async (navigate: NavigateFunction): Promise<void> => {
    const signinResponse = await this.authenticationService.signinRedirectCallback();

    if (signinResponse) {
      this.user = signinResponse;
      this.authenticationService.startSilentRenew();
    } else {
      await this.authenticationService.clearStaleState();
    }

    this.replaceLocation(navigate);
    this.removeRedirectLocation();
  };

  public signinSilent = async (): Promise<void> => {
    const silentResponse = await this.authenticationService.signinSilent();

    if (silentResponse) {
      this.user = silentResponse;
      console.log(`User '${this.user?.profile.sub}' successfully signed in silently!`);
    } else {
      console.log('Signin silent request is not successfull!');
    }
  };

  public signinSilentCallback = async (): Promise<void> => {
    await this.authenticationService.signinSilentCallback();
  };

  public signoutCallback = async (navigate: NavigateFunction): Promise<void> => {
    await this.authenticationService.signoutCallback();
    await this.authenticationService.clearStaleState();
    this.user = null;

    if (this.getSavedLocation()) {
      this.replaceLocation(navigate);
      this.removeRedirectLocation();
    }
  };

  public signoutPopup = async (location?: string): Promise<void> => {
    await this.authenticationService.signoutPopup();
  };

  public signoutPopupCallback = async (): Promise<void> => {
    await this.authenticationService.signoutPopupCallback();
    await this.authenticationService.clearStaleState();
    this.user = null;
  };

  public signoutRedirect = async (location?: string): Promise<void> => {
    this.saveLocation(location);
    await this.authenticationService.signoutRedirect();
  };

  public signoutRedirectCallback = async (navigate: NavigateFunction): Promise<void> => {
    await this.authenticationService.signoutRedirectCallback();
    await this.authenticationService.clearStaleState();
    this.user = null;
    this.replaceLocation(navigate);
    this.removeRedirectLocation();
  };
}
