import 'reflect-metadata';

import { Typography } from '@mui/material';
import { ReactElement } from 'react';

interface Properties {
  message?: string | undefined;
}

function SuccessMessage(properties: Properties): ReactElement {
  return (
    <Typography
      maxWidth={620}
      style={{
        color: 'green',
        fontSize: 14,
        fontWeight: 700,
        overflowWrap: 'break-word',
      }}
    >
      {properties.message ?? undefined}
    </Typography>
  );
}

export default SuccessMessage;
