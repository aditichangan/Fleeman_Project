import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import ApiService from '../../services/api';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import {
    Car,
    ArrowLeft,
    Search,
    Loader2,
    CheckCircle2,
    AlertCircle,
    MapPin,
    Calendar
} from "lucide-react";

const FleetOverview = () => {
    const navigate = useNavigate();
    const [fleetData, setFleetData] = useState(null);
    const [expandedHubs, setExpandedHubs] = useState({});
    const [searchTerm, setSearchTerm] = useState('');
    const [statusFilter, setStatusFilter] = useState('All');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        loadFleetOverview();
    }, []);

    const loadFleetOverview = async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await ApiService.getFleetOverview();
            if (!data) throw new Error("No data received from API");

            setFleetData(data);

            // Expand all hubs by default if they have cars
            const expanded = {};
            if (data.hubs) {
                data.hubs.forEach(hub => {
                    if (hub.cars && hub.cars.length > 0) expanded[hub.hubId] = true;
                });
            }
            setExpandedHubs(expanded);
        } catch (e) {
            console.error('Failed to load fleet overview:', e);
            setError(e.response?.data?.message || e.message || 'Failed to load fleet data. Please try again later.');
        } finally {
            setLoading(false);
        }
    };

    const toggleHub = (hubId) => {
        setExpandedHubs(prev => ({ ...prev, [hubId]: !prev[hubId] }));
    };

    const getFilteredCars = (cars) => {
        if (!cars) return [];
        return cars.filter(car => {
            const matchesSearch =
                (car.model?.toLowerCase().includes(searchTerm.toLowerCase())) ||
                (car.registrationNumber?.toLowerCase().includes(searchTerm.toLowerCase()));
            const matchesStatus = statusFilter === 'All' || car.status === statusFilter;
            return matchesSearch && matchesStatus;
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

    if (loading) {
        return (
            <div className="min-h-screen bg-muted/30 flex flex-col items-center justify-center p-6 text-center">
                <Loader2 className="h-12 w-12 text-primary animate-spin mb-4" />
                <h2 className="text-xl font-black uppercase tracking-widest text-muted-foreground animate-pulse">Analyzing Fleet Status...</h2>
            </div>
        );
    }

    if (error) {
        return (
            <div className="min-h-screen bg-muted/30 flex flex-col items-center justify-center p-6 text-center">
                <div className="h-20 w-20 rounded-full bg-destructive/10 flex items-center justify-center text-destructive mb-6">
                    <AlertCircle className="h-10 w-10" />
                </div>
                <h2 className="text-2xl font-black uppercase mb-2">Sync Error</h2>
                <p className="text-muted-foreground max-w-md mb-8">{error}</p>
                <Button onClick={loadFleetOverview} className="rounded-xl px-8 py-6 h-auto font-black uppercase tracking-widest text-xs shadow-xl">
                    Retry Connection
                </Button>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-muted/30 py-12">
            <div className="container mx-auto px-6 max-w-7xl">
                <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-12 gap-6 animate-in fade-in slide-in-from-top-4 duration-700">
                    <div>
                        <Button
                            variant="ghost"
                            onClick={() => navigate('/admin/dashboard')}
                            className="p-0 hover:bg-transparent text-primary font-bold gap-2 mb-4 h-auto"
                        >
                            <ArrowLeft className="h-4 w-4" /> Back to Intelligence Console
                        </Button>
                        <h1 className="text-4xl md:text-5xl font-black tracking-tight uppercase">
                            Fleet <span className="text-primary italic">Intelligence</span>
                        </h1>
                        <p className="text-muted-foreground mt-2 font-medium">Real-time suriveillance & logistical overview of the global inventory.</p>
                    </div>

                    <div className="flex flex-wrap gap-4">
                        <div className="bg-emerald-500/10 border border-emerald-500/20 px-6 py-4 rounded-2xl shadow-sm">
                            <p className="text-[10px] font-black uppercase text-muted-foreground tracking-widest mb-1">Available Fleet</p>
                            <p className="text-3xl font-black text-emerald-600">{fleetData?.statistics.totalAvailable}</p>
                        </div>
                        <div className="bg-blue-500/10 border border-blue-500/20 px-6 py-4 rounded-2xl shadow-sm">
                            <p className="text-[10px] font-black uppercase text-muted-foreground tracking-widest mb-1">Active Rentals</p>
                            <p className="text-3xl font-black text-blue-600">{fleetData?.statistics.totalRented}</p>
                        </div>
                        <div className="bg-amber-500/10 border border-amber-500/20 px-6 py-4 rounded-2xl shadow-sm">
                            <p className="text-[10px] font-black uppercase text-muted-foreground tracking-widest mb-1">In Maintenance</p>
                            <p className="text-3xl font-black text-amber-600">{fleetData?.statistics.totalMaintenance}</p>
                        </div>
                        <div className="bg-primary/10 border border-primary/20 px-6 py-4 rounded-2xl shadow-sm">
                            <p className="text-[10px] font-black uppercase text-muted-foreground tracking-widest mb-1">Utilization</p>
                            <p className="text-3xl font-black text-primary">{fleetData?.statistics.utilizationRate}%</p>
                        </div>
                    </div>
                </div>

                <Card className="border-none shadow-2xl bg-card overflow-hidden">
                    <CardHeader className="p-8 bg-muted/50 border-b border-border/50">
                        <div className="flex flex-col md:flex-row gap-6">
                            <div className="relative flex-grow group">
                                <Search className="absolute left-4 top-1/2 -translate-y-1/2 h-5 w-5 text-muted-foreground group-focus-within:text-primary transition-colors" />
                                <Input
                                    placeholder="Search by model, make, or registration number..."
                                    value={searchTerm}
                                    onChange={(e) => setSearchTerm(e.target.value)}
                                    className="h-14 pl-12 bg-background border-none rounded-2xl text-lg font-medium shadow-inner placeholder:font-normal"
                                />
                            </div>
                            <div className="flex flex-wrap gap-2">
                                {[
                                    { label: 'All Vehicles', value: 'All', color: 'primary' },
                                    { label: 'Available', value: 'Available', color: 'emerald' },
                                    { label: 'Rented', value: 'Rented', color: 'blue' },
                                    { label: 'In Service', value: 'Maintenance', color: 'amber' }
                                ].map((item) => (
                                    <Button
                                        key={item.value}
                                        variant={statusFilter === item.value ? 'default' : 'outline'}
                                        onClick={() => setStatusFilter(item.value)}
                                        className={`h-14 px-6 rounded-2xl font-black uppercase tracking-widest text-[10px] transition-all border-2
                                            ${statusFilter === item.value
                                                ? 'shadow-lg shadow-primary/20 scale-105 z-10'
                                                : 'bg-background border-border/50 hover:border-primary/50'
                                            }`}
                                    >
                                        {item.label}
                                    </Button>
                                ))}
                            </div>
                        </div>
                    </CardHeader>
                    <CardContent className="p-8">
                        <div className="space-y-6">
                            {fleetData?.hubs.map((hub) => {
                                // Dynamic Visibility: Only show hubs that contain vehicles matching the criteria
                                if (statusFilter === 'Available' && hub.availableCars === 0) return null;
                                if (statusFilter === 'Rented' && hub.rentedCars === 0) return null;
                                if (statusFilter === 'Maintenance' && hub.maintenanceCars === 0) return null;

                                const filteredCars = getFilteredCars(hub.cars);
                                // Hide if search is active and no matching vehicles in this hub
                                if (filteredCars.length === 0 && searchTerm) return null;

                                return (
                                    <div key={hub.hubId} className="rounded-3xl border-2 border-border/50 overflow-hidden bg-card/50 transition-all hover:border-primary/20">
                                        <div
                                            className="p-6 bg-muted/30 cursor-pointer flex items-center justify-between group"
                                            onClick={() => toggleHub(hub.hubId)}
                                        >
                                            <div className="flex items-center gap-4">
                                                <div className="h-14 w-14 rounded-2xl bg-primary/10 flex items-center justify-center text-primary group-hover:bg-primary group-hover:text-white transition-all">
                                                    <MapPin className="h-6 w-6" />
                                                </div>
                                                <div>
                                                    <h3 className="text-xl font-black uppercase tracking-tight">{hub.hubName}</h3>
                                                    <p className="text-xs text-muted-foreground font-bold uppercase tracking-widest">{hub.cityName || 'Regional Hub'}</p>
                                                </div>
                                            </div>
                                            <div className="flex items-center gap-6">
                                                <div className="hidden lg:flex gap-3">
                                                    <Badge className="bg-emerald-500/10 text-emerald-600 hover:bg-emerald-500/10 border-emerald-500/20 font-black text-[10px] uppercase px-3 py-1.5 tracking-tighter">
                                                        {hub.availableCars} Ready
                                                    </Badge>
                                                    <Badge className="bg-blue-500/10 text-blue-600 hover:bg-blue-500/10 border-blue-500/20 font-black text-[10px] uppercase px-3 py-1.5 tracking-tighter">
                                                        {hub.rentedCars} Out
                                                    </Badge>
                                                    <Badge className="bg-amber-500/10 text-amber-600 hover:bg-amber-500/10 border-amber-500/20 font-black text-[10px] uppercase px-3 py-1.5 tracking-tighter">
                                                        {hub.maintenanceCars} Service
                                                    </Badge>
                                                </div>
                                                <div className={`text-muted-foreground transition-transform duration-300 ${expandedHubs[hub.hubId] ? 'rotate-180' : ''}`}>
                                                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round"><path d="m6 9 6 6 6-6" /></svg>
                                                </div>
                                            </div>
                                        </div>

                                        {expandedHubs[hub.hubId] && (
                                            <div className="p-0 border-t border-border/50 animate-in slide-in-from-top-2 duration-300">
                                                <div className="overflow-x-auto">
                                                    <table className="w-full text-left border-collapse">
                                                        <thead className="bg-muted/10">
                                                            <tr className="text-[10px] font-black uppercase tracking-widest text-muted-foreground border-b border-border/50">
                                                                <th className="px-8 py-5">Vehicle Identification</th>
                                                                <th className="px-8 py-5 text-center">Class</th>
                                                                <th className="px-8 py-5 text-center">Status</th>
                                                                <th className="px-8 py-5 text-right">Yield (Daily)</th>
                                                                <th className="px-8 py-5">Current Engagement</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody className="divide-y divide-border/30">
                                                            {filteredCars.map((car) => (
                                                                <tr key={car.carId} className="group hover:bg-muted/30 transition-colors">
                                                                    <td className="px-8 py-6">
                                                                        <div>
                                                                            <p className="font-black text-base uppercase tracking-tight">{car.model}</p>
                                                                            <p className="font-mono text-[10px] font-black text-muted-foreground uppercase">{car.registrationNumber}</p>
                                                                        </div>
                                                                    </td>
                                                                    <td className="px-8 py-6 text-center">
                                                                        <Badge variant="outline" className="font-black text-[10px] uppercase tracking-widest border-2">
                                                                            {car.carType || 'Fleet'}
                                                                        </Badge>
                                                                    </td>
                                                                    <td className="px-8 py-6 text-center">
                                                                        <Badge className={`${getStatusColor(car.status)} font-black text-[10px] uppercase tracking-widest border-2 px-4 py-1.5`}>
                                                                            {car.status}
                                                                        </Badge>
                                                                    </td>
                                                                    <td className="px-8 py-6 text-right">
                                                                        <p className="font-black text-lg text-primary">
                                                                            {car.dailyRate ? `₹${car.dailyRate.toLocaleString()}` : '—'}
                                                                        </p>
                                                                    </td>
                                                                    <td className="px-8 py-6">
                                                                        {car.currentRental ? (
                                                                            <div className="bg-blue-500/5 border border-blue-500/10 rounded-xl p-3">
                                                                                <p className="font-black text-xs text-blue-700 uppercase mb-1">{car.currentRental.customerName}</p>
                                                                                <div className="flex items-center gap-4 text-[10px] font-bold text-muted-foreground uppercase">
                                                                                    <span className="flex items-center gap-1"><Calendar className="h-3 w-3" /> {new Date(car.currentRental.startDate).toLocaleDateString()}</span>
                                                                                    <span>→</span>
                                                                                    <span>{new Date(car.currentRental.endDate).toLocaleDateString()}</span>
                                                                                </div>
                                                                            </div>
                                                                        ) : (
                                                                            <div className="flex items-center gap-2 text-muted-foreground/30 italic text-sm">
                                                                                <CheckCircle2 className="h-4 w-4" /> No active deployment
                                                                            </div>
                                                                        )}
                                                                    </td>
                                                                </tr>
                                                            ))}
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        )}
                                    </div>
                                );
                            })}
                            {fleetData?.hubs.length === 0 && (
                                <div className="p-20 text-center bg-muted/20 rounded-3xl border-2 border-dashed border-border/50">
                                    <Car className="h-16 w-16 text-muted-foreground mx-auto mb-6 opacity-20" />
                                    <h3 className="text-2xl font-black uppercase text-muted-foreground opacity-50 tracking-widest">No fleet data synchronized</h3>
                                    <p className="text-muted-foreground mt-2">Connect with hubs or upload vehicle inventory to populate the matrix.</p>
                                </div>
                            )}
                        </div>
                    </CardContent>
                </Card>
            </div>
        </div>
    );
};

export default FleetOverview;
