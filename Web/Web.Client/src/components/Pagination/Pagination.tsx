import 'reflect-metadata';

import { Pagination as MUIPagination } from '@mui/material';
import { observer } from 'mobx-react';
import { ChangeEvent, ReactElement, useState } from 'react';
import { useNavigate } from 'react-router-dom';

interface Properties {
  totalCount: number;
  currentPage: number;
  onChange: (event: ChangeEvent<unknown>, value: number) => void;
}

const Pagination = observer(({ totalCount, currentPage, onChange }: Properties): ReactElement => {
  const navigate = useNavigate();

  const [page, setPage] = useState(currentPage);

  const handleChange = (event: ChangeEvent<unknown>, value: number): void => {
    setPage(value);
    onChange(event, value);
    const urlParameters = new URLSearchParams(window.location.search);

    if (value > 1) {
      urlParameters.set('page', value.toString());
    } else {
      urlParameters.delete('page');
    }

    navigate('?' + urlParameters.toString(), { replace: false, preventScrollReset: true });
  };

  return totalCount > 0 ? <MUIPagination count={totalCount} page={page} onChange={handleChange} /> : <></>;
});

export default Pagination;
