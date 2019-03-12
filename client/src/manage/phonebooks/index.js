import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import _ from 'lodash';
import AutoComplete from 'react-autocomplete';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import PhoneBook from './PhoneBookItem';
import Pagination from '../../common/components/utils/Pagination';
import Spinner from '../../common//components/utils/Spinner';
import history from '../../common/utils/history';
import Api from '../../api';

import { getPhoneBooks } from '../actions';

class PhoneBooks extends Component {
	state = {
		value: '',
		autocompleteData: []
	};
	static propTypes = {
		getPhoneBooks: PropTypes.func.isRequired,
		phoneBooks: PropTypes.object,
		pagingInfo: PropTypes.object
	};

	componentDidMount() {
		const page = this.props.location.pathname;
		if (page) {
			if (page.includes('page')) {
				this.props.getPhoneBooks(page.slice(-1), 3);
			} else {
				this.props.getPhoneBooks(1, 3);
			}
		} else {
			this.props.getPhoneBooks(1, 3);
		}
	}

	componentDidUpdate(prevProps) {
		if (prevProps.location.pathname !== this.props.location.pathname) {
			const page = this.props.location.pathname;
			if (page) {
				if (page.includes('page')) {
					this.props.getPhoneBooks(page.slice(-1), 3);
				} else {
					this.props.getPhoneBooks(1, 3);
				}
			} else {
				this.props.getPhoneBooks(1, 3);
			}
		}
	}

	onChange = e => {
		this.setState({
			value: e.target.value
		});
		if (e.target.value) {
			this.retrieveDataAsynchronously(e.target.value);
		} else {
			this.setState({ autocompleteData: [] });
		}
	};

	onSelect = val => {
		this.setState({
			value: val
		});
	};

	renderItem(item, isHighlighted) {
		return (
			<div
				className='card my-1'
				style={{ background: isHighlighted ? 'lightgray' : 'white', zIndex: '1000' }}
				key={item.phoneBookId}
			>
				<div className='card-header'>{`Found in ${item.data[0].phoneBook.title}`}</div>
				<ul className='list-group list-group-flush'>
					{item.data.map(d => (
						<li
							className='list-group-item'
							key={d.id}
							onClick={() =>
								history.push(`/manage/phonebook/${item.data[0].phoneBook.id}/entries/${d.id}`)
							}
							style={{ cursor: 'pointer' }}
						>
							<h5 className='mb-2 font-weight-light text-primary text-center text-md'>
								{`${d.firstName} ${d.lastName}`}{' '}
							</h5>

							<ul className='list-group list-group-horizontal list-group-flush d-flex justify-content-center'>
								<li className='list-group-item border-0 text-primary'>
									<FontAwesomeIcon icon='phone' /> {d.phoneNumber}
								</li>
								<li className='list-group-item border-0 text-primary text-left'>
									<FontAwesomeIcon icon='at' /> {d.emailAddress}
								</li>
								<li className='list-group-item border-0 text-primary'>
									<FontAwesomeIcon icon='map-marked-alt' /> {d.address}
								</li>
							</ul>
						</li>
					))}
				</ul>
			</div>
		);
	}

	getItemValue(item) {
		return `${item}`;
	}

	retrieveDataAsynchronously = _.debounce(async searchText => {
		const response = await Api.searchForContact(searchText);
		const data = _.groupBy(response.data, 'phoneBook.id');
		const items = [];
		_.forEach(data, item => {
			items.push({ phoneBookId: item[0].phoneBook.id, data: item });
		});

		this.setState({ autocompleteData: items });
	}, 300);

	renderList() {
		const list = _.values(this.props.phoneBooks).map(phoneBook => {
			return (
				<PhoneBook
					title={phoneBook.title}
					description={phoneBook.description}
					key={phoneBook.id}
					id={phoneBook.id}
				/>
			);
		});

		return (
			<>
				{list}
				<Pagination url={this.props.match.url} pagingInfo={this.props.pagingInfo} />
			</>
		);
	}

	render() {
		if (!this.props.phoneBooks || !this.props.pagingInfo) {
			return <Spinner />;
		}

		return (
			<div style={{ height: '80.3vh' }}>
				<div className='container'>
					<h1 className='pt-4 mb-4'>Your Phone Books</h1>
					<div className='d-flex justify-content-between mb-2 position-relative'>
						<button
							className='btn btn-primary'
							onClick={() => {
								history.push(`/manage/phonebook/new`);
							}}
						>
							Add Phone Book
						</button>
						<AutoComplete
							wrapperStyle={{ display: 'inline-block' }}
							renderInput={props => (
								<>
									<input
										{...props}
										className='form-control form-control-lg'
										placeholder='Search for contacts'
										style={{ width: '350px' }}
									/>
									<FontAwesomeIcon
										icon='search'
										size='2x'
										className='text-primary'
										style={{ position: 'absolute', top: '8px', right: '10px' }}
									/>
								</>
							)}
							menuStyle={{
								borderRadius: '3px',
								boxShadow: '0 2px 12px rgba(0, 0, 0, 0.1)',
								background: 'rgba(255, 255, 255, 0.9)',
								padding: '2px',
								fontSize: '90%',
								position: 'fixed',
								overflow: 'auto',
								maxHeight: '50%', // TODO: don't cheat, let it flow to the bottom,
								zIndex: '5'
							}}
							getItemValue={this.getItemValue}
							items={this.state.autocompleteData}
							renderItem={this.renderItem}
							value={this.state.value}
							onChange={this.onChange}
							onSelect={this.onSelect}
						/>
					</div>
					{this.renderList()}
				</div>
			</div>
		);
	}
}

const mapStateToProps = ({ phoneBooks: { items, pagingInfo }, searchResults }) => {
	return {
		phoneBooks: items,
		pagingInfo: pagingInfo,
		searchResults
	};
};

export default connect(
	mapStateToProps,
	{ getPhoneBooks }
)(PhoneBooks);
