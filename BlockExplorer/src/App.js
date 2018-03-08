import React, {Component} from 'react';
import './App.css';
import {Route} from 'react-router-dom';
import Header from './components/Header/Header';
import Footer from './components/Footer/Footer';
import Home from './components/Home/Home';
import Blocks from './components/Blocks/Blocks';
import Block from './components/Block/Block';
import Transaction from './components/Transaction/Transaction';

class App extends Component {
    render() {
        return (
            <div className="App">
                <Header/>

                <Route exact={true} path="/" component={Home}/>
                <Route path="/blocks" component={Blocks}/>
                <Route path="/blocks/:id" component={Block}/>
                <Route path="/transactions/:id" component={Transaction}/>

                <Footer/>
            </div>
        );
    }
}

export default App;
