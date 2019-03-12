import { combineReducers } from 'redux';

import errorReducer from './errorReducer';
import authReducer from '../auth/reducers';
import phoneBooksReducer from '../manage/reducers';

export default combineReducers({
	errors: errorReducer,
	auth: authReducer,
	phoneBooks: phoneBooksReducer
});
