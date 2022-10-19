/* eslint-disable @typescript-eslint/no-empty-function */

import 'reflect-metadata';

import { useMediaQuery } from '@mui/material';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { observer } from 'mobx-react';
import { createContext, useMemo, useState } from 'react';

interface Props {
  children?: React.ReactNode;
}

export const ColorModeContext = createContext({ toggleColorMode: () => {} });

const ThemeChanger = observer(({ children }: Props) => {
  let colorModeSetup = localStorage.getItem('colorModeSetup');

  if (colorModeSetup === null) {
    const prefersDarkMode = useMediaQuery('(prefers-color-scheme: dark)');
    colorModeSetup = prefersDarkMode ? 'dark' : 'light';
    localStorage.setItem('colorModeSetup', colorModeSetup);
  }

  const [mode, setMode] = useState<'light' | 'dark'>(colorModeSetup === 'light' ? 'light' : 'dark');

  const colorMode = useMemo(
    () => ({
      toggleColorMode: (): void => {
        localStorage.setItem('colorModeSetup', mode === 'light' ? 'dark' : 'light');
        setMode((prevMode) => (prevMode === 'light' ? 'dark' : 'light'));
      },
    }),
    [mode]
  );

  const theme = useMemo(
    () =>
      createTheme({
        palette: {
          mode,
        },
        shape: {
          borderRadius: 18,
        },
      }),
    [mode]
  );

  return (
    <ColorModeContext.Provider value={colorMode}>
      <ThemeProvider theme={theme}>{children}</ThemeProvider>
    </ColorModeContext.Provider>
  );
});

export default ThemeChanger;
