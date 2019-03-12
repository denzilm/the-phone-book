import _ from 'lodash';

import { SET_CURRENT_USER } from '../constants/action-types';

const initialState = {
	authenticated: false,
	user: {}
};

export default (state = initialState, action) => {
	switch (action.type) {
		case SET_CURRENT_USER:
			return { ...state, authenticated: !_.isEmpty(action.payload), user: action.payload };
		default:
			return state;
	}
};
