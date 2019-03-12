import _ from 'lodash';

import {
	GET_PHONE_BOOKS,
	GET_PHONE_BOOK_WITH_ENTRIES,
	GET_PHONE_BOOK_ENTRY,
	DELETE_PHONE_BOOK_ENTRY,
	DELETE_PHONE_BOOK
} from '../constants/action-types';

const initialState = { items: null, pagingInfo: null, searchResults: [] };

export default (state = initialState, action) => {
	switch (action.type) {
		case GET_PHONE_BOOKS:
			const { phoneBooks, pagingInfo } = action.payload;
			return { ...state, items: phoneBooks, pagingInfo };
		case GET_PHONE_BOOK_WITH_ENTRIES:
			return { ...state, items: { [action.payload.id]: action.payload } };
		case GET_PHONE_BOOK_ENTRY:
			return {
				...state,
				items: {
					[action.payload.phoneBookId]: {
						id: action.payload.phoneBookId,
						phoneBookEntries: [action.payload.entry]
					}
				}
			};
		case DELETE_PHONE_BOOK_ENTRY:
			const entries = _.filter(
				_.values(state.items).filter(p => p.id === action.payload.phoneBookId)[0].phoneBookEntries,
				entry => entry.id !== action.payload.id
			);

			return {
				...state,
				items: {
					[action.payload.phoneBookId]: {
						...state.items[action.payload.phoneBookId],
						phoneBookEntries: entries
					}
				}
			};
		case DELETE_PHONE_BOOK:
			const items = _.omit(state.items, action.payload);
			return { ...state, items };
		default:
			return state;
	}
};
