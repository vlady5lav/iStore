import 'reflect-metadata';

import 'styles.scss';

import { Provider as IoCProvider } from 'inversify-react';
import { configure as configureMobX } from 'mobx';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';

import { App } from 'containers/App';
import { IoCContainer } from 'ioc';
import reportWebVitals from 'reportWebVitals';
import * as serviceWorkerRegistration from 'serviceWorkerRegistration';

configureMobX({
  enforceActions: 'never',
});

const container = document.querySelector('#root');
const root = createRoot(container!);

root.render(
  <StrictMode>
    <IoCProvider container={IoCContainer}>
      <App />
    </IoCProvider>
  </StrictMode>
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
serviceWorkerRegistration.unregister();

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
void reportWebVitals();
