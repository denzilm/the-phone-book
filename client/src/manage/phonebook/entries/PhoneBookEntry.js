import React, { Component } from 'react';
import { Form, Field } from 'react-final-form';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { connect } from 'react-redux';
import _ from 'lodash';

import history from '../../../common/utils/history';
import * as Validators from '../../../common/validation';
import Input from '../../../common/components/form/Input';
import TextArea from '../../../common/components/form/TextArea';
import Spinner from '../../../common/components/utils/Spinner';
import { getPhoneBookEntry, updatePhoneBookEntry, createPhoneBookEntry } from '../../actions';

class PhoneBookEntry extends Component {
	componentDidMount() {
		if (this.props.match.params.id !== 'new') {
			this.props.getPhoneBookEntry(this.props.match.params.phoneBookId, this.props.match.params.id);
		}
	}

	onSubmit = values => {
		if (this.props.match.params.id === 'new') {
			this.props.createPhoneBookEntry(this.props.match.params.phoneBookId, values);
			console.log(values);
		} else {
			this.props.updatePhoneBookEntry(this.props.phoneBookId, values);
		}
	};

	render() {
		if (this.props.match.params.id !== 'new' && !this.props.phoneBookEntry) {
			return <Spinner />;
		}

		return (
			<div style={{ height: '80.3vh' }}>
				<div className='container'>
					<div
						className='d-flex align-items-center'
						onClick={() => history.goBack()}
					>
						<FontAwesomeIcon
							icon='chevron-left'
							className='text-primary mt-2'
							size='3x'
							style={{ cursor: 'pointer' }}
						/>
					</div>

					<h3 className='pt-4 mb-4 display-4 lead'>
						{this.props.match.params.id === 'new'
							? 'Creating New Contact'
							: `Editing contact details for: ${this.props.phoneBookEntry[0].firstName} ${
									this.props.phoneBookEntry[0].lastName
							  }`}
					</h3>
					<Form
						onSubmit={this.onSubmit}
						initialValues={this.props.match.params.id === 'new' ? null : this.props.phoneBookEntry[0]}
						render={({ handleSubmit, submitting, pristine, invalid }) => (
							<form onSubmit={handleSubmit} style={{ paddingBottom: '100px' }}>
								<Field
									name='firstName'
									placeholder='First Name'
									component={Input}
									validate={Validators.required('First Name')}
								/>
								<Field
									name='lastName'
									placeholder='Last Name'
									component={Input}
									validate={Validators.composeValidators(Validators.required('Last Name'))}
								/>
								<Field
									name='emailAddress'
									placeholder='Email Address'
									component={Input}
									validate={Validators.composeValidators(
										Validators.required('Email Address'),
										Validators.isEmail
									)}
								/>
								<Field
									name='phoneNumber'
									placeholder='Phone Number'
									component={Input}
									type={'tel'}
									pattern={'[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-s./0-9]*'}
									validate={Validators.composeValidators(
										Validators.required('Phone Number'),
										Validators.isPhoneNumber
									)}
								/>
								<Field
									name='address'
									placeholder='Address'
									component={TextArea}
									validate={Validators.composeValidators(Validators.required('Address'))}
									cols={1}
									row={3}
								/>
								<div className='text-center'>
									<input
										type='submit'
										className='btn btn-primary mt-4 btn-lg btn-block'
										disabled={submitting || pristine || invalid}
										value={'Save'}
									/>
								</div>
							</form>
						)}
					/>
				</div>
			</div>
		);
	}
}

const mapStateToProps = (state, ownProps) => {
	const phoneBook = _.values(state.phoneBooks.items).filter(p => p.id === ownProps.match.params.phoneBookId)[0];
	console.log(phoneBook);
	if (phoneBook && phoneBook.phoneBookEntries) {
		return {
			phoneBookEntry: phoneBook.phoneBookEntries.filter(e => e.id === ownProps.match.params.id),
			phoneBookId: phoneBook.id
		};
	} else {
		return {
			phoneBookEntry: null
		};
	}
};

export default connect(
	mapStateToProps,
	{ getPhoneBookEntry, updatePhoneBookEntry, createPhoneBookEntry }
)(PhoneBookEntry);
