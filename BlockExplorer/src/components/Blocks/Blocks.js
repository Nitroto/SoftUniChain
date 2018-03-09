import React, {Component} from 'react';
import './Block.sass';
import axios from 'axios/index';
import Moment from 'react-moment';
import {Link} from 'react-router-dom';
import Currency from 'react-currency-formatter';

class Blocks extends Component {
    constructor(props) {
        super(props);

        this.state = {
            blocks: [],
            now: new Date()
        };

        this.sumValuesOfTransactions = this.sumValuesOfTransactions.bind(this)
    }

    sumValuesOfTransactions(transactions) {
        let sum = 0;
        transactions.forEach(function (tx) {
            sum += tx.value
        });
        return sum / 1000000
    }

    componentDidMount() {
        axios.get('http://localhost:5000/api/blocks').then(res => {
            const blocks = res.data;
            this.setState({blocks});
        })
    }

    render() {
        return (
            <div>
                <section className="section">
                    <div className="container">
                        <h1 className="title">List of blocks</h1>
                        {this.state.blocks.length > 0 &&
                        <table className="table has-text-centered is-hoverable is-fullwidth">
                            <thead>
                            <tr>
                                <th><abbr title="Height"></abbr>Height</th>
                                <th><abbr title="Age"></abbr>Age</th>
                                <th><abbr title="Transactions"></abbr>Transactions</th>
                                <th><abbr title="Total Sent"></abbr>Total Sent</th>
                            </tr>
                            </thead>
                            <tbody>
                            {this.state.blocks.reverse().map(block =>
                                <tr key={block.index}>
                                    <td><Link to={'/blocks/' + block.blockHash}>{block.index}</Link></td>
                                    <td>
                                        <Moment fromNow ago>{block.createdOn}</Moment>
                                    </td>
                                    <td>{block.transactions.length}</td>
                                    <td>
                                        <Currency
                                            quantity={this.sumValuesOfTransactions(block.transactions)}
                                            pattern="##,###.00### !"
                                            symbol="SUC"
                                            decimal="."/>
                                    </td>
                                </tr>
                            )}
                            </tbody>
                        </table>}
                    </div>
                </section>
            </div>
        );
    }
}

export default Blocks;
