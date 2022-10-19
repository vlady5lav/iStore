import 'reflect-metadata';

import { Grid, Typography } from '@mui/material';
import { observer } from 'mobx-react';
import { ChangeEvent, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation } from 'react-router-dom';

import { LoadingSpinner } from 'components/LoadingSpinner';
import { Pagination } from 'components/Pagination';
import { ProductCard } from 'components/ProductCard';
import { SelectorBrand } from 'components/SelectorBrand';
import { SelectorType } from 'components/SelectorType';
import { IoCTypes, useInjection } from 'ioc';
import { ProductsStore } from 'stores';

const Products = observer(() => {
  const store = useInjection<ProductsStore>(IoCTypes.productsStore);
  const location = useLocation();
  const { t } = useTranslation(['products']);

  useEffect(() => {
    const getProducts = async (): Promise<void> => {
      await store.getItems();
    };

    getProducts().catch((error) => {
      if (error instanceof Error) {
        console.error(error.message);
      } else {
        console.log(error);
      }
    });
  }, [store, store.currentPage, store.pageLimit, store.selectedBrandIds, store.selectedTypeIds, location]);

  return (
    <Grid key={Math.random() * 12_345} container justifyContent="center" marginY={4} marginX={0.5}>
      <Grid key={Math.random() * 12_345} container justifyContent="center">
        <Grid key={Math.random() * 12_345} item mb={4} ml={2} mr={2}>
          <SelectorBrand
            label={t('selectors.brands')}
            items={store.brands}
            selectedBrandIds={store.selectedBrandIds}
            minWidth={250}
            onChange={store.changeBrandIds}
          />
        </Grid>
        <Grid key={Math.random() * 12_345} item mb={4} ml={2} mr={2}>
          <SelectorType
            label={t('selectors.types')}
            items={store.types}
            selectedTypeIds={store.selectedTypeIds}
            minWidth={250}
            onChange={store.changeTypeIds}
          />
        </Grid>
      </Grid>
      {store.isLoading ? (
        <LoadingSpinner />
      ) : (
        <Grid key={Math.random() * 12_345} container justifyContent="center">
          {store.products.length > 0 ? (
            store.products?.map((product) => (
              <Grid key={Math.random() * 12_345} item mb={4} ml={2} mr={2}>
                <ProductCard product={product} />
              </Grid>
            ))
          ) : (
            <Typography whiteSpace="pre-line">{t('placeholder.empty')}</Typography>
          )}
        </Grid>
      )}
      <Grid container justifyContent="center">
        <Pagination
          totalCount={store.totalPages}
          currentPage={store.currentPage}
          onChange={(event: ChangeEvent<unknown>, value: number): void => {
            store.changePage(value);
          }}
        />
      </Grid>
    </Grid>
  );
});

export default Products;
