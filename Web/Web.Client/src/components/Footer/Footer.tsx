import 'reflect-metadata';

import { Link, Paper, Stack, Typography } from '@mui/material';
import { observer } from 'mobx-react';

import { LanguageChangerButton } from 'components/LanguageChangerButton';
import { LimitChangerButton } from 'components/LimitChangerButton';
import { ThemeChangerButton } from 'components/ThemeChangerButton';

const Copyright = (): JSX.Element => {
  return (
    <Typography variant="body2" color="text.secondary">
      {'Copyright © '}
      <Link color="inherit" href={process.env.PUBLIC_URL}>
        {document.title}
      </Link>{' '}
      {new Date().getFullYear()}
      {'.'}
    </Typography>
  );
};

const Footer = observer((): JSX.Element => {
  return (
    <Paper
      elevation={1}
      sx={{
        borderRadius: '0px',
        display: 'grid',
        justifyContent: 'center',
        justifyItems: 'center',
        alignContent: 'center',
        alignItems: 'center',
        py: 1,
        px: 1,
        mt: 'auto',
      }}
    >
      <Stack direction="column">
        <Stack
          direction="row"
          spacing={{ xs: 1, sm: 2, md: 4 }}
          justifyContent="center"
          justifyItems="center"
          alignContent="center"
          alignItems="center"
        >
          <LanguageChangerButton height={40} width={40} borderRadius={90} />
          <ThemeChangerButton />
          <LimitChangerButton />
        </Stack>
        <Stack
          mt={1}
          spacing={{ xs: 1, sm: 2, md: 4 }}
          justifyContent="center"
          justifyItems="center"
          alignContent="center"
          alignItems="center"
        >
          <Copyright />
        </Stack>
      </Stack>
    </Paper>
  );
});

export default Footer;
