import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import history from '../../common/utils/history';
import { deletePhoneBook } from '../actions';

const PhoneBookItem = props => {
	return (
		<div
			className='card border-success mb-2 phone-book overflow-auto'
			onClick={() => history.push(`/manage/phonebook/${props.id}`)}
		>
			<div className='card-header d-flex justify-content-between'>
				<h2 className='h2'>{props.title}</h2>
				<button
					className='btn btn-danger align-self-center remove'
					onClick={evt => {
						evt.stopPropagation();
						props.deletePhoneBook(props.id);
					}}
				>
					Remove
				</button>
			</div>
			<div className='card-body' style={{ maxHeight: '120px' }}>
				<div className='d-flex'>
					<p className='card-text lead text-normal' style={{ width: '90%' }}>
						{props.description}
					</p>
					<Link to={`/manage/phonebook/${props.id}`} className='btn btn-primary ml-auto align-self-center'>
						<FontAwesomeIcon icon='sign-in-alt' className='text-white' size='2x' />
					</Link>
				</div>
			</div>
		</div>
	);
};

export default connect(
	null,
	{ deletePhoneBook }
)(PhoneBookItem);
