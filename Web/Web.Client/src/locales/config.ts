import { init, use } from 'i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import { initReactI18next } from 'react-i18next';

import consts from './consts/consts.json';
import appEn from './en/app.json';
import cartEn from './en/cart.json';
import checkoutEn from './en/checkout.json';
import headerEn from './en/header.json';
import productEn from './en/product.json';
import productsEn from './en/products.json';
import profileEn from './en/profile.json';
import appUk from './uk/app.json';
import cartUk from './uk/cart.json';
import checkoutUk from './uk/checkout.json';
import headerUk from './uk/header.json';
import productUk from './uk/product.json';
import productsUk from './uk/products.json';
import profileUk from './uk/profile.json';

const resources = {
  en: {
    app: appEn,
    cart: cartEn,
    checkout: checkoutEn,
    header: headerEn,
    product: productEn,
    products: productsEn,
    profile: profileEn,
    consts: consts,
  },
  uk: {
    app: appUk,
    cart: cartUk,
    checkout: checkoutUk,
    header: headerUk,
    product: productUk,
    products: productsUk,
    profile: profileUk,
    consts: consts,
  },
};

use(initReactI18next);
use(LanguageDetector);

init({
  detection: {
    // order and from where user language should be detected
    order: [
      'querystring',
      'cookie',
      'localStorage',
      'sessionStorage',
      'navigator',
      'htmlTag',
      // 'path',
      // 'subdomain',
    ],

    // keys or params to lookup language from
    lookupQuerystring: 'lng',
    lookupCookie: 'i18next',
    lookupLocalStorage: 'i18nextLng',
    lookupSessionStorage: 'i18nextLng',
    // lookupFromPathIndex: 0,
    // lookupFromSubdomainIndex: 0,

    // cache user language on
    caches: ['localStorage' /*, 'cookie'*/],
    excludeCacheFor: ['cimode'], // languages to not persist (cookie, localStorage)

    // optional expire and domain for set cookie
    // cookieMinutes: 10,
    // cookieDomain: 'myDomain',

    // optional htmlTag with lang attribute, the default is:
    // htmlTag: document.documentElement,

    // optional set cookie options, reference:[MDN Set-Cookie docs](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie)
    // cookieOptions: { path: '/', sameSite: 'strict' },
  },

  // lng: 'en',
  fallbackLng: ['en', 'uk'],
  ns: ['app', 'cart', 'checkout', 'header', 'product', 'products', 'profile', 'consts'],
  defaultNS: 'consts',
  supportedLngs: ['en', 'uk'],
  interpolation: {
    escapeValue: false, // react already safes from xss
  },
  debug: false,
  /*
    react: {
      bindI18n: 'languageChanged',
      bindI18nStore: '',
      transEmptyNodeValue: '',
      transKeepBasicHtmlNodesFor: ['br', 'strong', 'i'],
      transSupportBasicHtmlNodes: true,
      useSuspense: true,
    },
    */
  resources,
}).catch((error) => {
  if (error instanceof Error) {
    console.error(error.message);
  } else {
    console.log(error);
  }
});

export { default } from 'i18next';
