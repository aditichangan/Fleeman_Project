import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion';
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Car, MapPin, IndianRupee, Search, Filter, ArrowRight, Loader2 } from "lucide-react";
import ApiService from '@/services/api';

const ExploreVehicles = () => {
    const navigate = useNavigate();
    const [fleetData, setFleetData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [selectedHub, setSelectedHub] = useState('all');
    const [searchTerm, setSearchTerm] = useState('');
    const [statusFilter, setStatusFilter] = useState('Available');

    useEffect(() => {
        loadFleetData();
    }, []);

    const loadFleetData = async () => {
        try {
            setLoading(true);
            const data = await ApiService.getFleetOverview();
            setFleetData(data);
        } catch (error) {
            console.error('Failed to load fleet data:', error);
        } finally {
            setLoading(false);
        }
    };

    const getFilteredVehicles = () => {
        if (!fleetData) return [];

        let allCars = [];

        // Collect all cars from all hubs
        fleetData.hubs.forEach(hub => {
            hub.cars.forEach(car => {
                allCars.push({
                    ...car,
                    hubId: hub.hubId,
                    hubName: hub.hubName,
                    cityName: hub.cityName
                });
            });
        });

        // Apply filters
        return allCars.filter(car => {
            const matchesHub = selectedHub === 'all' || car.hubId === parseInt(selectedHub);
            const matchesSearch = car.model.toLowerCase().includes(searchTerm.toLowerCase()) ||
                car.registrationNumber.toLowerCase().includes(searchTerm.toLowerCase());
            const matchesStatus = statusFilter === 'All' || car.status === statusFilter;

            return matchesHub && matchesSearch && matchesStatus;
        });
    };

    const getStatusColor = (status) => {
        switch (status) {
            case 'Available': return 'bg-emerald-500/10 text-emerald-600 border-emerald-500/20';
            case 'Rented': return 'bg-blue-500/10 text-blue-600 border-blue-500/20';
            case 'Maintenance': return 'bg-amber-500/10 text-amber-600 border-amber-500/20';
            default: return 'bg-gray-500/10 text-gray-600 border-gray-500/20';
        }
    };

    const handleBookNow = (car) => {
        // Navigate to booking page with pre-selected hub
        navigate(`/booking?hubId=${car.hubId}&carId=${car.carId}`);
    };

    if (loading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-background">
                <div className="text-center space-y-4">
                    <Loader2 className="h-12 w-12 animate-spin text-primary mx-auto" />
                    <p className="text-muted-foreground">Loading vehicles...</p>
                </div>
            </div>
        );
    }

    const filteredVehicles = getFilteredVehicles();

    return (
        <div className="min-h-screen bg-background py-24">
            <div className="container mx-auto px-4">
                {/* Header */}
                <motion.div
                    initial={{ opacity: 0, y: -20 }}
                    animate={{ opacity: 1, y: 0 }}
                    className="text-center mb-12"
                >
                    <h1 className="text-5xl md:text-6xl font-black tracking-tight mb-4">
                        Explore Our <span className="text-primary">Fleet</span>
                    </h1>
                    <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
                        Browse our premium collection of vehicles across all locations
                    </p>
                </motion.div>

                {/* Filters */}
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ delay: 0.2 }}
                    className="mb-12"
                >
                    <Card className="border-none shadow-xl bg-card">
                        <CardContent className="p-6">
                            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                                {/* Hub Filter */}
                                <div className="space-y-2">
                                    <label className="text-sm font-bold uppercase tracking-wider text-muted-foreground flex items-center gap-2">
                                        <MapPin className="h-4 w-4" />
                                        Location
                                    </label>
                                    <select
                                        value={selectedHub}
                                        onChange={(e) => setSelectedHub(e.target.value)}
                                        className="flex h-12 w-full rounded-xl bg-muted border-none px-4 py-2 text-sm font-medium focus-visible:ring-2 focus-visible:ring-primary"
                                    >
                                        <option value="all">All Locations</option>
                                        {fleetData?.hubs.map(hub => (
                                            <option key={hub.hubId} value={hub.hubId}>
                                                {hub.hubName} - {hub.cityName}
                                            </option>
                                        ))}
                                    </select>
                                </div>

                                {/* Search */}
                                <div className="space-y-2">
                                    <label className="text-sm font-bold uppercase tracking-wider text-muted-foreground flex items-center gap-2">
                                        <Search className="h-4 w-4" />
                                        Search
                                    </label>
                                    <Input
                                        type="text"
                                        placeholder="Search by model or registration..."
                                        value={searchTerm}
                                        onChange={(e) => setSearchTerm(e.target.value)}
                                        className="h-12 bg-muted border-none rounded-xl"
                                    />
                                </div>

                                {/* Status Filter */}
                                <div className="space-y-2">
                                    <label className="text-sm font-bold uppercase tracking-wider text-muted-foreground flex items-center gap-2">
                                        <Filter className="h-4 w-4" />
                                        Status
                                    </label>
                                    <select
                                        value={statusFilter}
                                        onChange={(e) => setStatusFilter(e.target.value)}
                                        className="flex h-12 w-full rounded-xl bg-muted border-none px-4 py-2 text-sm font-medium focus-visible:ring-2 focus-visible:ring-primary"
                                    >
                                        <option value="All">All Status</option>
                                        <option value="Available">Available</option>
                                        <option value="Rented">Rented</option>
                                        <option value="Maintenance">Maintenance</option>
                                    </select>
                                </div>
                            </div>

                            {/* Results Count */}
                            <div className="mt-4 pt-4 border-t border-border">
                                <p className="text-sm text-muted-foreground">
                                    Showing <span className="font-bold text-primary">{filteredVehicles.length}</span> vehicles
                                </p>
                            </div>
                        </CardContent>
                    </Card>
                </motion.div>

                {/* Vehicle Grid */}
                {filteredVehicles.length === 0 ? (
                    <div className="text-center py-20">
                        <Car className="h-20 w-20 text-muted-foreground/20 mx-auto mb-4" />
                        <h3 className="text-2xl font-bold mb-2">No vehicles found</h3>
                        <p className="text-muted-foreground">Try adjusting your filters</p>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {filteredVehicles.map((car, index) => (
                            <motion.div
                                key={car.carId}
                                initial={{ opacity: 0, y: 20 }}
                                animate={{ opacity: 1, y: 0 }}
                                transition={{ delay: index * 0.05 }}
                            >
                                <Card className="border-none shadow-xl bg-card overflow-hidden hover:shadow-2xl transition-all duration-300 group">
                                    {/* Car Image */}
                                    <div className="relative h-48 bg-gradient-to-br from-primary/10 to-primary/5 flex items-center justify-center overflow-hidden">
                                        {car.imagePath ? (
                                            <img
                                                src={`/${car.imagePath}`}
                                                alt={car.model}
                                                className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
                                                onError={(e) => {
                                                    e.target.style.display = 'none';
                                                    e.target.nextSibling.style.display = 'flex';
                                                }}
                                            />
                                        ) : null}

                                        {/* Fallback Placeholder (shown if no image or error) */}
                                        <div className={`absolute inset-0 flex items-center justify-center ${car.imagePath ? 'hidden' : 'flex'}`}>
                                            <Car className="h-24 w-24 text-primary/20 group-hover:scale-110 transition-transform duration-500" />
                                        </div>

                                        <div className="absolute top-4 right-4">
                                            <Badge className={`${getStatusColor(car.status)} font-black text-xs px-3 py-1 shadow-md`}>
                                                {car.status}
                                            </Badge>
                                        </div>
                                    </div>

                                    <CardContent className="p-6 space-y-4">
                                        {/* Vehicle Info */}
                                        <div>
                                            <h3 className="text-xl font-black mb-1">{car.model}</h3>
                                            <p className="text-sm text-muted-foreground">{car.carType || 'Standard'}</p>
                                        </div>

                                        {/* Details */}
                                        <div className="space-y-2">
                                            <div className="flex items-center gap-2 text-sm">
                                                <MapPin className="h-4 w-4 text-primary" />
                                                <span className="font-medium">{car.hubName}</span>
                                                <span className="text-muted-foreground">• {car.cityName}</span>
                                            </div>
                                            <div className="flex items-center gap-2 text-sm">
                                                <span className="font-mono font-bold text-muted-foreground">{car.registrationNumber}</span>
                                            </div>
                                        </div>

                                        {/* Price */}
                                        {car.dailyRate && (
                                            <div className="flex items-baseline gap-1 pt-2 border-t border-border">
                                                <IndianRupee className="h-5 w-5 text-primary" />
                                                <span className="text-3xl font-black text-primary">{car.dailyRate.toLocaleString()}</span>
                                                <span className="text-sm text-muted-foreground">/day</span>
                                            </div>
                                        )}

                                        {/* Rental Info (if rented) */}
                                        {car.currentRental && (
                                            <div className="bg-blue-500/10 border border-blue-500/20 rounded-xl p-3">
                                                <p className="text-xs font-bold text-blue-600 mb-1">Currently Rented</p>
                                                <p className="text-xs text-muted-foreground">
                                                    {new Date(car.currentRental.startDate).toLocaleDateString()} - {new Date(car.currentRental.endDate).toLocaleDateString()}
                                                </p>
                                            </div>
                                        )}

                                        {/* Book Button */}
                                        <Button
                                            onClick={() => handleBookNow(car)}
                                            disabled={car.status !== 'Available'}
                                            className="w-full h-12 rounded-xl font-black uppercase tracking-widest text-xs shadow-lg shadow-primary/20 gap-2 group/btn"
                                        >
                                            {car.status === 'Available' ? (
                                                <>
                                                    Book Now
                                                    <ArrowRight className="h-4 w-4 group-hover/btn:translate-x-1 transition-transform" />
                                                </>
                                            ) : (
                                                'Not Available'
                                            )}
                                        </Button>
                                    </CardContent>
                                </Card>
                            </motion.div>
                        ))}
                    </div>
                )}

                {/* Statistics Summary */}
                {fleetData && (
                    <motion.div
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        transition={{ delay: 0.5 }}
                        className="mt-16"
                    >
                        <Card className="border-none shadow-xl bg-card">
                            <CardHeader>
                                <CardTitle className="text-center">Fleet Statistics</CardTitle>
                            </CardHeader>
                            <CardContent>
                                <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
                                    <div className="text-center">
                                        <div className="text-3xl font-black text-primary mb-1">{fleetData.statistics.totalCars}</div>
                                        <div className="text-sm text-muted-foreground uppercase tracking-wider">Total Vehicles</div>
                                    </div>
                                    <div className="text-center">
                                        <div className="text-3xl font-black text-emerald-600 mb-1">{fleetData.statistics.totalAvailable}</div>
                                        <div className="text-sm text-muted-foreground uppercase tracking-wider">Available</div>
                                    </div>
                                    <div className="text-center">
                                        <div className="text-3xl font-black text-blue-600 mb-1">{fleetData.statistics.totalRented}</div>
                                        <div className="text-sm text-muted-foreground uppercase tracking-wider">Rented</div>
                                    </div>
                                    <div className="text-center">
                                        <div className="text-3xl font-black text-primary mb-1">{fleetData.statistics.utilizationRate}%</div>
                                        <div className="text-sm text-muted-foreground uppercase tracking-wider">Utilization</div>
                                    </div>
                                </div>
                            </CardContent>
                        </Card>
                    </motion.div>
                )}
            </div>
        </div>
    );
};

export default ExploreVehicles;
