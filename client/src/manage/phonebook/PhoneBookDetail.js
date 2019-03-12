import React, { Component } from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import Spinner from '../../common/components/utils/Spinner';
import { getPhoneBookWithEntries, updatePhoneBook, deletePhoneBookEntry } from '../actions';
import history from '../../common/utils/history';
import ContactItem from './ContactItem';
import PhoneBook from './PhoneBook';

class PhoneBookDetail extends Component {
	componentDidMount() {
		if (this.props.match.params.id) {
			this.props.getPhoneBookWithEntries(this.props.match.params.id);
		}
	}

	renderContacts() {
		if (this.props.phoneBook.phoneBookEntries.length < 1) {
			return (
				<div className='card-body'>
					<p className='lead'>Please add some contacts</p>
				</div>
			);
		} else {
			return (
				<>
					<div className='list-group  list-group-flush'>
						{this.props.phoneBook.phoneBookEntries.map(entry => (
							<ContactItem
								address={entry.address}
								email={entry.emailAddress}
								name={`${entry.firstName} ${entry.lastName}`}
								phoneNumber={entry.phoneNumber}
								url={`/manage/phonebook/${this.props.phoneBook.id}/entries/${entry.id}`}
								deletePhoneBookEntry={this.props.deletePhoneBookEntry}
								id={entry.id}
								phoneBookId={this.props.phoneBook.id}
								key={entry.id}
							/>
						))}
					</div>
				</>
			);
		}
	}

	render() {
		console.log(this.props);
		if (!this.props.phoneBook || !this.props.phoneBook.phoneBookEntries) {
			return <Spinner />;
		}

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

					<div className='card mt-5' style={{ marginBottom: '100px' }}>
						<div className='card-header d-flex justify-content-between'>
							<p className='text-md d-flex justify-content-center align-items-center'>Contacts</p>
							<button
								className='btn btn-primary align-self-center'
								onClick={() => {
									history.push(`/manage/phonebook/${this.props.match.params.id}/entries/new`);
								}}
							>
								Add Contact
							</button>
						</div>
						{this.renderContacts()}
					</div>
				</div>
			</div>
		);
	}
}

const mapStateToProps = (state, ownProps) => {
	return { phoneBook: _.values(state.phoneBooks.items).filter(p => p.id === ownProps.match.params.id)[0] };
};

export default connect(
	mapStateToProps,
	{ getPhoneBookWithEntries, updatePhoneBook, deletePhoneBookEntry }
)(PhoneBookDetail);
