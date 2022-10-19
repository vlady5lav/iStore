import 'reflect-metadata';

import Button from '@mui/material/Button';
import { useTheme } from '@mui/material/styles';
import { observer } from 'mobx-react';
import { useState } from 'react';

import i18n from 'locales/config';

interface Properties {
  height?: number;
  width?: number;
  borderRadius?: string | number;
}

const LanguageChangerButton = observer(({ height, width, borderRadius }: Properties) => {
  const theme = useTheme();
  const [language, setLanguage] = useState<string>(i18n.resolvedLanguage);
  const supportedLngs = i18n.store.options.supportedLngs ?? i18n.languages ?? [language];
  const languages: string[] = (Array.isArray(supportedLngs) ? supportedLngs : [language]).filter((val, idx, arr) => {
    return val !== 'cimode';
  });

  const currentLanguageIndex = (): number => languages.indexOf(language);

  const nextLanguage = (): string => {
    const currentLanguage = currentLanguageIndex();

    return languages[currentLanguage < languages.length - 1 ? currentLanguage + 1 : 0];
  };

  const handleChange = async (): Promise<void> => {
    const targetLanguage = nextLanguage();
    setLanguage(targetLanguage);
    await i18n.changeLanguage(targetLanguage);
  };

  const displayLanguage = nextLanguage().toUpperCase();

  return (
    <Button
      sx={{
        height: height ? height : 'auto',
        width: width ? width : 'auto',
        minWidth: width ? width : 'auto',
        borderRadius: borderRadius ? borderRadius : theme.shape.borderRadius,
      }}
      className="langSwitcher"
      variant="contained"
      color="info"
      onClick={async (): Promise<void> => handleChange()}
    >
      {displayLanguage}
    </Button>
  );
});

export default LanguageChangerButton;
