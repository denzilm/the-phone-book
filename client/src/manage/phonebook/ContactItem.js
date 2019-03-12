import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import history from '../../common/utils/history';

export default ({ name, phoneNumber, email, address, url, phoneBookId, id, deletePhoneBookEntry }) => {
	return (
		<>
			<div
				className='list-group-item list-group-item-action flex-column align-items-start phone-book-entry'
				onClick={() => history.push(url)}
			>
				<h5 className='mb-2 font-weight-light text-primary text-center text-md'>
					{name}{' '}
					<button
						className='btn btn-danger align-self-center remove float-right'
						onClick={evt => {
							evt.stopPropagation();
							deletePhoneBookEntry(phoneBookId, id);
						}}
					>
						Remove
					</button>
				</h5>

				<ul className='list-group list-group-horizontal list-group-flush d-flex justify-content-center'>
					<li className='list-group-item border-0 text-primary'>
						<FontAwesomeIcon icon='phone' /> {phoneNumber}
					</li>
					<li className='list-group-item border-0 text-primary text-left'>
						<FontAwesomeIcon icon='at' /> {email}
					</li>
					<li className='list-group-item border-0 text-primary'>
						<FontAwesomeIcon icon='map-marked-alt' /> {address}
					</li>
				</ul>
			</div>
		</>
	);
};
