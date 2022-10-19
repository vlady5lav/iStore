import { Button, CircularProgress } from '@mui/material';
import { ReactElement } from 'react';

interface Properties {
  isLoading: boolean;
  text?: string;
  disabled?: string | boolean;
  type?: 'button' | 'submit' | 'reset' | undefined;
  onClick: () => void;
  onChange: () => void;
  variant?: 'text' | 'outlined' | 'contained' | undefined;
}

const ButtonSpinner = ({ isLoading, text, disabled, type, onClick, onChange, variant }: Properties): ReactElement => {
  return (
    <Button
      variant={variant ? variant : 'outlined'}
      disabled={Boolean(disabled)}
      type={type}
      onClick={(): void => {
        onClick();
      }}
      onChange={(): void => {
        onChange();
      }}
    >
      {isLoading ? <CircularProgress /> : `${text}`}
    </Button>
  );
};

export default ButtonSpinner;
