import { watchBag } from './tea/bag';
import { watchBagSearch } from './tea/search';
import { watchCountry } from './country';
import { watchBagType } from './tea/bagtype';
import { watchBrand } from './tea/brand';
import { watchDashboard } from './dashboard/chart';

export default function* rootSaga() {
  yield [
    watchBag(),
    watchBagSearch(),
    watchBrand(),
    watchBagType(),
    watchCountry(),
    watchDashboard(),
  ];
}

