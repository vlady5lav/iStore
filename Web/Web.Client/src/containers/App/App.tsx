import 'reflect-metadata';

import { CssBaseline } from '@mui/material';
import { observer } from 'mobx-react';

import { ThemeChanger } from 'components/ThemeChanger';
import { AppRoutes } from 'routes';

const App = observer(() => {
  return (
    <ThemeChanger>
      <CssBaseline />
      <AppRoutes />
    </ThemeChanger>
  );
});

export default App;
