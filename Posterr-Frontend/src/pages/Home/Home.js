import React, { useState, useEffect, useContext, useRef, useCallback } from 'react';
import api from '../../services/api';
import PostForm from '../../components/PostForm/PostForm';
import Post from '../../components/Post/Post';
import Header from '../../components/Header/Header';
import './home.css';
import { FaRegEdit } from "react-icons/fa";
import { AiOutlineRetweet } from "react-icons/ai";
import { UserContext } from '../../contexts/userContext';
import { showSuccessMessage, showErrorMessage, repostMessage, repostWithQuoteMessage } from '../../utils/sweetalertUtils';

const Home = () => {
    const [posts, setPosts] = useState([]);
    const [newPostContent, setNewPostContent] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [filter, setFilter] = useState('');
    const [sortOrder, setSortOrder] = useState('latest');
    const [currentPage, setCurrentPage] = useState(1); // Track current page
    const [totalPosts, setTotalPosts] = useState(0); // Track total posts
    const { selectedUser } = useContext(UserContext);
    const observer = useRef();

    // Fetch posts when sortOrder or filter changes
    useEffect(() => {
        fetchPosts(1, 15);
    }, [sortOrder, filter]);

    const fetchPosts = async (page, take) => {
        setLoading(true);
        const skip = (page - 1) * take;
        try {
            const res = await api.get('/posts', {
                params: {
                    skip: skip,
                    take: take,
                    sortBy: sortOrder,
                    keyword: filter
                }
            });
            setPosts(prevPosts => (page === 1 ? res.data.posts : [...prevPosts, ...res.data.posts]));
            setCurrentPage(res.data.currentPage);
            setTotalPosts(res.data.totalPosts);
            setLoading(false);
        } catch (error) {
            setError(error.message);
            setLoading(false);
            showErrorMessage(`Failed to fetch posts: ${error.message}`);
        }
    };

    const handleCreatePost = async () => {
        const post = { content: newPostContent, userId: selectedUser.id };
        try {
            await api.post('/posts', post);
            setNewPostContent('');
            showSuccessMessage('Post created successfully!');
            fetchPosts(1, posts.length + 1); // Fetch all posts including the new one
        } catch (error) {
            showErrorMessage(`Failed to create post: ${error.response.data}`);
        }
    };

    const handleRepost = async (postId) => {
        let result = await repostMessage('Are you sure you want to repost this?');
        if (result.isConfirmed) {
            const post = { content: null, userId: selectedUser.id, postId: postId };
            try {
                await api.post(`/posts/repost`, post);
                showSuccessMessage('Post reposted successfully!');
                fetchPosts(1, posts.length); // Refetch posts
            } catch (error) {
                showErrorMessage(`Failed to repost: ${error.response.data}`);
            }
        }
    };

    const handleRepostQuote = async (postId) => {
        let result = await repostWithQuoteMessage('Are you sure you want to repost this?');
        if (result.isConfirmed) {
            const post = { content: result.value, userId: selectedUser.id , postId: postId };
            try {
                await api.post(`/posts/repost`, post);
                showSuccessMessage('Post reposted successfully!');
                fetchPosts(1, posts.length); // Refetch posts
            } catch (error) {
                showErrorMessage(`Failed to repost: ${error.response.data}`);
            }
        }
    };

    // Infinite scrolling observer
    const lastPostElementRef = useCallback(node => {
        if (loading) return; // Don't fetch if already loading
        if (observer.current) observer.current.disconnect();
        observer.current = new IntersectionObserver(entries => {
            if (entries[0].isIntersecting && posts.length < totalPosts) {
                fetchPosts(currentPage + 1, 20); // Fetch next page with 20 posts
            }
        });
        if (node) observer.current.observe(node);
    }, [loading, currentPage, totalPosts]);

    const handleSearch = (e) => {
        if (e.key === 'Enter') {
            fetchPosts(1, 15); // Fetch posts with entered keyword
        }
    };

    return (
        <div>
            <Header />
            <h1>Posterr</h1>
            <PostForm
                newPostContent={newPostContent}
                handleCreatePost={handleCreatePost}
                updateNewPostContent={setNewPostContent}
            />
            <div>
                <input
                    type="text"
                    placeholder="Search..."
                    value={filter}
                    onChange={(e) => setFilter(e.target.value)}
                    onKeyPress={handleSearch}
                />
                <select onChange={(e) => setSortOrder(e.target.value)} value={sortOrder}>
                    <option value="latest">Latest</option>
                    <option value="trending">Trending</option>
                </select>
            </div>
            {loading && posts.length === 0 ? (
                <p>Loading...</p>
            ) : error ? (
                <p>Error: {error}</p>
            ) : (
                <div>
                    {posts.map((post, index) => {
                        const repostId = post.id;
                        if (posts.length === index + 1) {
                            return (
                                <div key={post.id} className="post-wrapper" ref={lastPostElementRef}>
                                    <Post post={post} />
                                    {!post.originalPost && (
                                        <div className="post-buttons-container">
                                            <button onClick={() => handleRepost(repostId)}>
                                                <AiOutlineRetweet size={20} />
                                            </button>
                                            <button onClick={() => handleRepostQuote(repostId)}>
                                                <FaRegEdit size={20} />
                                            </button>
                                        </div>
                                    )}
                                </div>
                            );
                        } else {
                            return (
                                <div key={post.id} className="post-wrapper">
                                    <Post post={post} />
                                    {!post.originalPost && (
                                        <div className="post-buttons-container">
                                            <button onClick={() => handleRepost(repostId)}>
                                                <AiOutlineRetweet size={20} />
                                            </button>
                                            <button onClick={() => handleRepostQuote(repostId)}>
                                                <FaRegEdit size={20} />
                                            </button>
                                        </div>
                                    )}
                                </div>
                            );
                        }
                    })}
                </div>
            )}
            {loading && posts.length !== 0 && <p>Loading more posts...</p>}
        </div>
    );
};

export default Home;
