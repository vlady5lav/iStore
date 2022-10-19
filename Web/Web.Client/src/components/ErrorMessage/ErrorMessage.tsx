import 'reflect-metadata';

import { Typography } from '@mui/material';
import { ReactElement } from 'react';

interface Properties {
  error?: string | undefined;
}

function ErrorMessage(properties: Properties): ReactElement {
  return (
    <Typography
      maxWidth={620}
      style={{
        color: 'red',
        fontSize: 14,
        fontWeight: 700,
        overflowWrap: 'break-word',
      }}
    >
      {properties.error ?? undefined}
    </Typography>
  );
}

export default ErrorMessage;
