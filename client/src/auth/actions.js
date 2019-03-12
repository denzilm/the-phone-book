import jwt_decode from 'jwt-decode';

import { SET_CURRENT_USER, SET_ERRORS } from '../constants/action-types';
import history from '../common/utils/history';

import Api from '../api';

export const registerUser = user => async dispatch => {
	try {
		await Api.registerUser(user);
		history.push('/login');
	} catch (error) {
		dispatch({ type: SET_ERRORS, payload: error.response.data });
	}
};

export const loginUser = (email, password) => async dispatch => {
	try {
		const response = await Api.loginUser(email, password);
		localStorage.setItem('token', response.data.accessToken.token);
		const decoded = jwt_decode(response.data.accessToken.token);
		dispatch(setCurrentUser(decoded));
	} catch (error) {
		dispatch({
			type: SET_ERRORS,
			payload: [{ code: 'Unauthorized', description: 'Your username and/or password is incorrect' }]
		});
	}
};

export const setCurrentUser = user => {
	return {
		type: SET_CURRENT_USER,
		payload: user
	};
};

export const logoutUser = () => dispatch => {
	localStorage.removeItem('token');
	dispatch(setCurrentUser({}));
};
