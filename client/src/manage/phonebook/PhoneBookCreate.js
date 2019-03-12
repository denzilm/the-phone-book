import React, { Component } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { connect } from 'react-redux';

import PhoneBook from './PhoneBook';
import history from '../../common/utils/history';
import { createPhoneBook } from '../actions';

class PhoneBookCreate extends Component {
	render() {
		return (
			<div style={{ height: '80.3vh' }}>
				<div className='container'>
					<div className='d-flex align-items-center' onClick={() => history.goBack()}>
						<FontAwesomeIcon
							icon='chevron-left'
							className='text-primary mt-2'
							size='3x'
							style={{ cursor: 'pointer' }}
						/>
					</div>
					<PhoneBook {...this.props} />
				</div>
			</div>
		);
	}
}

export default connect(
	null,
	{ createPhoneBook }
)(PhoneBookCreate);
