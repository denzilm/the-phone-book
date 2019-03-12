import _ from 'lodash';

import {
	GET_PHONE_BOOKS,
	SET_ERRORS,
	GET_PHONE_BOOK_WITH_ENTRIES,
	GET_PHONE_BOOK_ENTRY,
	UPDATE_PHONE_BOOK_ENTRY,
	CREATE_PHONE_BOOK_ENTRY,
	DELETE_PHONE_BOOK_ENTRY,
	UPDATE_PHONE_BOOK,
	CREATE_PHONE_BOOK,
	DELETE_PHONE_BOOK
} from '../constants/action-types';
import Api from '../api';
import history from '../common/utils/history';

export const getPhoneBooks = (page, pageSize) => async dispatch => {
	try {
		const response = await Api.getBooks(page, pageSize);
		const payload = {
			phoneBooks: _.mapKeys(response.data, 'id'),
			pagingInfo: JSON.parse(response.headers['x-pagination'])
		};
		dispatch({ type: GET_PHONE_BOOKS, payload });
	} catch (error) {
		dispatch({ type: SET_ERRORS, payload: error.response.data });
	}
};

export const getPhoneBookWithEntries = id => async dispatch => {
	try {
		const response = await Api.getBookWithEntries(id);
		dispatch({ type: GET_PHONE_BOOK_WITH_ENTRIES, payload: response.data });
	} catch (error) {
		dispatch({ type: SET_ERRORS, payload: error.response.data });
	}
};

export const getPhoneBookEntry = (phoneBookId, id) => async dispatch => {
	try {
		const response = await Api.getBookEntry(phoneBookId, id);
		dispatch({ type: GET_PHONE_BOOK_ENTRY, payload: { phoneBookId, entry: response.data } });
	} catch (error) {
		dispatch({ type: SET_ERRORS, payload: error.response.data });
	}
};

export const updatePhoneBookEntry = (phoneBookId, phoneBookEntry) => async dispatch => {
	try {
		const response = await Api.updatePhoneBookEntry(phoneBookId, phoneBookEntry.id, _.omit(phoneBookEntry, 'id'));
		dispatch({ type: UPDATE_PHONE_BOOK_ENTRY, payload: { phoneBookId, entry: response.data } });
		history.push(`/manage/phonebook/${phoneBookId}`);
	} catch (error) {
		dispatch({ type: SET_ERRORS, payload: error.response.data });
	}
};

export const createPhoneBookEntry = (phoneBookId, phoneBookEntry) => async dispatch => {
	try {
		const response = await Api.createPhoneBookEntry(phoneBookId, phoneBookEntry);
		dispatch({ type: CREATE_PHONE_BOOK_ENTRY, payload: response.data });
		history.push(`/manage/phonebook/${phoneBookId}`);
	} catch (error) {
		dispatch({ type: SET_ERRORS, payload: error.response.data });
	}
};

export const deletePhoneBookEntry = (phoneBookId, id) => async dispatch => {
	try {
		await Api.deletePhoneBookEntry(phoneBookId, id);
		dispatch({ type: DELETE_PHONE_BOOK_ENTRY, payload: { phoneBookId, id } });
	} catch (error) {
		console.log(error);
		dispatch({ type: SET_ERRORS, payload: error.response.data });
	}
};

export const updatePhoneBook = phoneBook => async dispatch => {
	try {
		const response = await Api.updateBook(phoneBook.id, _.omit(phoneBook, 'id', 'phoneBookEntries'));
		dispatch({ type: UPDATE_PHONE_BOOK, payload: response.data });
		history.push('/manage/phonebooks');
	} catch (error) {
		dispatch({ type: SET_ERRORS, payload: error.response.data });
	}
};

export const createPhoneBook = phoneBook => async dispatch => {
	try {
		const response = await Api.createPhoneBook(phoneBook);
		dispatch({ type: CREATE_PHONE_BOOK, payload: response.data });
		history.push('/manage/phonebooks');
	} catch (error) {
		dispatch({ type: SET_ERRORS, payload: error.response.data });
	}
};

export const deletePhoneBook = id => async dispatch => {
	try {
		await Api.deletePhoneBook(id);
		dispatch({ type: DELETE_PHONE_BOOK, payload: id });
	} catch (error) {
		dispatch({ type: SET_ERRORS, payload: error.response.data });
	}
};