import * as React from 'react';
import {Component, ComponentClass} from 'react';

export interface IItemListProps<T> {
  items: T[];
  renderItem(item: T): JSX.Element;
  onItemSelected(item: T): void;
}

export interface IItemsListState {
  itemHoverIndex: number;
}

export function ItemsList<T>(): ComponentClass<IItemListProps<T>> {
  return class extends Component<IItemListProps<T>, IItemsListState> {
    controls: {
      input?: HTMLInputElement;
    } = {};

    constructor(props: IItemListProps<T>) {
      super(props);

      this.onKeyDown = this.onKeyDown.bind(this);
      this.onMouseLeave = this.onMouseLeave.bind(this);
      this.onClick = this.onClick.bind(this);

      this.state = {itemHoverIndex: props.items.length > 0 ? 0 : -1}
    }

    componentDidMount() {
      document.body.addEventListener('keydown', this.onKeyDown);
    }

    componentWillUnmount() {
      document.body.removeEventListener('keydown', this.onKeyDown);

    }

    onKeyDown(event) {
      switch (event.key) {
        case 'ArrowDown': {
          if(this.state.itemHoverIndex < this.props.items.length -1) {
            this.setState({itemHoverIndex: this.state.itemHoverIndex + 1});
          }
          event.stopPropagation();
          event.preventDefault();
          break;
        }
        case 'ArrowUp': {
          if(this.state.itemHoverIndex > 0) {
            this.setState({itemHoverIndex: this.state.itemHoverIndex - 1});
          }
          event.stopPropagation();
          event.preventDefault();
          break;
        }
        case 'Enter': {
          this.props.onItemSelected(this.props.items[this.state.itemHoverIndex]);
          event.stopPropagation();
          event.preventDefault();
          break;
        }
      }
    }

    onMouseEnter(index: number) {
      this.setState({itemHoverIndex: index});
    }

    onMouseLeave() {
      this.setState({itemHoverIndex: -1});
    }

    onClick() {
      this.props.onItemSelected(this.props.items[this.state.itemHoverIndex]);
    }

    renderItem(item: T, index: number) {
      let itemProps = {
         key:index,
         style:{padding: 6, cursor: 'pointer', zIndex: 3},
         className: `list-group-item ${this.state.itemHoverIndex === index ? 'list-group-item-info' : ''}`,
         onMouseEnter: this.onMouseEnter.bind(this, index),
         onMouseLeave: this.onMouseLeave,
         onClick: this.onClick
      };
      return <li {...itemProps} >{this.props.renderItem(item)}</li>;
    }

    render() {
      return  <ul style={{position: 'absolute', top: '100%', left: 0, backgroundColor: '#fff', width: '100%'}} className='list-group'>
                {this.props.items.map((item, index) => { return this.renderItem(item, index); })}
              </ul>;
    }
  }
}
