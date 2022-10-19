import 'reflect-metadata';

import { Box, CircularProgress, Stack, Typography } from '@mui/material';
import { ReactElement } from 'react';
import { useTranslation } from 'react-i18next';

function LoadingSpinner(): ReactElement {
  const { t } = useTranslation(['app']);

  return (
    <Box
      id="loadingSpinner"
      className="loadingSpinner"
      display="flex"
      textAlign="center"
      justifyContent="center"
      justifyItems="center"
      justifySelf="center"
      alignContent="center"
      alignItems="center"
      alignSelf="center"
      minHeight="100%"
      minWidth="100%"
    >
      <Stack
        display="flex"
        direction="row"
        spacing={2}
        textAlign="center"
        justifyContent="center"
        justifyItems="center"
        justifySelf="center"
        alignContent="center"
        alignItems="center"
        alignSelf="center"
      >
        <Stack
          display="flex"
          textAlign="center"
          justifyContent="center"
          justifyItems="center"
          justifySelf="center"
          alignContent="center"
          alignItems="center"
          alignSelf="center"
        >
          <Typography>{t('loading')}</Typography>
        </Stack>
        <Stack
          display="flex"
          textAlign="center"
          justifyContent="center"
          justifyItems="center"
          justifySelf="center"
          alignContent="center"
          alignItems="center"
          alignSelf="center"
        >
          <CircularProgress role="status" />
        </Stack>
      </Stack>
    </Box>
  );
}

export default LoadingSpinner;
