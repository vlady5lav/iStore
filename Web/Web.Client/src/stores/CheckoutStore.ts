import 'reflect-metadata';

import { injectable } from 'inversify';
import { makeAutoObservable } from 'mobx';

@injectable()
export default class CheckoutStore {
  firstName = '';
  lastName = '';
  address1 = '';
  address2 = '';
  city = '';
  state = '';
  zip = '';
  country = '';
  cardName = '';
  cardNumber: number | undefined = undefined;
  expDate = '';
  cvv: number | undefined = undefined;

  constructor() {
    this.firstName = '';
    this.lastName = '';
    this.address1 = '';
    this.address2 = '';
    this.city = '';
    this.state = '';
    this.zip = '';
    this.country = '';
    this.cardName = '';
    this.cardNumber = undefined;
    this.expDate = '';
    this.cvv = undefined;
    makeAutoObservable(this);
  }

  public init = (): void => {
    this.firstName = '';
    this.lastName = '';
    this.address1 = '';
    this.address2 = '';
    this.city = '';
    this.state = '';
    this.zip = '';
    this.country = '';
    this.cardName = '';
    this.cardNumber = undefined;
    this.expDate = '';
    this.cvv = undefined;
  };

  public changeFirstName = (text: string): void => {
    this.firstName = text;
  };

  public changeLastName = (text: string): void => {
    this.lastName = text;
  };

  public changeAddress1 = (text: string): void => {
    this.address1 = text;
  };

  public changeAddress2 = (text: string): void => {
    this.address2 = text;
  };

  public changeCity = (text: string): void => {
    this.city = text;
  };

  public changeState = (text: string): void => {
    this.state = text;
  };

  public changeZip = (text: string): void => {
    this.zip = text;
  };

  public changeCountry = (text: string): void => {
    this.country = text;
  };

  public changeCardName = (text: string): void => {
    this.cardName = text;
  };

  public changeCardNumber = (text: number): void => {
    this.cardNumber = text;
  };

  public changeExpDate = (text: string): void => {
    this.expDate = text;
  };

  public changeCvv = (text: number): void => {
    this.cvv = text;
  };
}
